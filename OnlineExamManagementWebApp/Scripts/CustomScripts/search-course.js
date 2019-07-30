// I used IIFE around the jQuery code to avoid conflict with other js libraries

(($) => {
    $(() => {
        $("#tbl-courseSearchResult").DataTable();

        const jsTrainerList = $("#TrainerId");

        $(document.body).on("change", "#OrganizationId", function () {
            const orgId = $(this).val();

            jsTrainerList.empty();
            jsTrainerList.append(`<option value="0">--SELECT TRAINER--</option>`);

            if (orgId != undefined && orgId !== "") {
                const json = { id: orgId };

                $.ajax({
                    type: "POST",
                    url: `/Course/GetTrainersByOrganization/${orgId}`,
                    contentType: "application/json",
                    dataType: "json",
                    data: JSON.stringify(json),

                    success: (data) => {
                        $.each(data, (key, value) => {
                            jsTrainerList.append(`<option value="${value.Id}"> ${value.Name} </option>`);
                        });
                    },

                    error: () => alert("Something is wrong")
                });
            }
        });

        /**
         *  Edit Course Modal PopUp
         */
        const editCourseModal = $("#js-modal-editCourse");
        const modalLeadTrainerSelect = $("#modal-editCourse-LeadTrainer");

        const getTrainersPromise = (courseId) => {
            return new Promise((resolve, reject) => {
                $.get(`/Course/GetTrainersByCourse/${courseId}`)
                    .done((data) => {
                        resolve(data);
                    });
            });
        }

        const fetchTrainers = (courseId) => {
            let trainers = [];
            getTrainersPromise(courseId).then((result) => {
                $.each(result, (key, value) => {
                    let jsonObject = {};
                    jsonObject["TrainerId"] = value.TrainerId;
                    jsonObject["TrainerName"] = value.TrainerName;
                    jsonObject["IsLead"] = value.IsLead;
                    trainers.push(jsonObject);
                });
            });
            return trainers;
        }

        const getTableRowAsObject = (tableRow) => {
            const courseId = parseInt(tableRow.find("td:eq(0)").text());
            let row = {
                Id: courseId,
                Name: tableRow.find("td:eq(1)").text(),
                Code: tableRow.find("td:eq(2)").text(),
                Duration: tableRow.find("td:eq(3)").text(),
                Credit: tableRow.find("td:eq(5)").text(),
                Outline: tableRow.find("td:eq(6)").text()
            }

            row["Trainers"] = fetchTrainers(courseId);
            return row;
        }

        const clearEditCourseModal = () => {
            modalLeadTrainerSelect.find("option")
                .remove()
                .end()
                .append(`<option value="">--SELECT TRAINER--</option>`);
        }

        const bindDataToEditCoursePopup = (data) => {
            $("#modal-editCourse-Name").val(data.Name);
            $("#modal-editCourse-Code").val(data.Code);
            $("#modal-editCourse-Duration").val(data.Duration);
            $("#modal-editCourse-Credit").val(data.Credit);
            $("#modal-editCourse-Outline").val(data.Outline);

            clearEditCourseModal();
            $.each(data.Trainers, (key, value) => {
                let appendString = `<option value=${value.TrainerId}>${value.TrainerName}</option>`;
                if (value.IsLead) {
                    modalLeadTrainerSelect.append(appendString).val(value.TrainerId);
                } else {
                    modalLeadTrainerSelect.append(appendString);
                }
            });
        }

        /**
         * Modal Update button functions
         */
        const btnModalUpdateCourse = $("#js-modal-btn-updateCourse");

        const getEditCourseModalContent = () => {
            let content = {
                Name: $("#modal-editCourse-Name").val(),
                Code: $("#modal-editCourse-Code").val(),
                Duration: $("#modal-editCourse-Duration").val(),
                Credit: $("#modal-editCourse-Credit").val(),
                Outline: $("#modal-editCourse-Outline").val(),
                LeadTrainerId: $("#modal-editCourse-LeadTrainer").children("option").filter(":selected").val()
            }
            console.log(content);
        }
        btnModalUpdateCourse.on("click", () => {
            getEditCourseModalContent();
        });

        /** END **/
        $(document).on("click", ".js-editCourseModalPopup", (event) => {
            const rowData = getTableRowAsObject($(event.target).closest("tr"));

            setTimeout(() => {
                editCourseModal.modal("toggle");
                bindDataToEditCoursePopup(rowData);
            }, 250);
        });

        /** End**/

        // jQuery Code Ends Here
    });
})(jQuery);