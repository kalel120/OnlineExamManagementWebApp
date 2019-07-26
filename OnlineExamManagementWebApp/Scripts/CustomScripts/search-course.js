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

        const promiseCourseTrainer = (courseId) => {
            return new Promise((resolve, reject) => {
                $.get(`/Course/GetTrainersByCourse/${courseId}`)
                    .done((data) => {
                        resolve(data);
                    });
            });
        };

        const fetchTrainers = (courseId) => {
            let trainers=[];
            promiseCourseTrainer(courseId).then((result) => {
                $.each(result, (key, value) => {
                    let jsonData = {};
                    jsonData["TrainerId"] = value.TrainerId;
                    jsonData["TrainerName"] = value.TrainerName;
                    trainers.push(jsonData);
                });              
            });
            return trainers;
        };

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
        const bindDataToEditCoursePopup = (data) => {
            $("#modal-editCourse-Name").val(data.Name);
            $("#modal-editCourse-Code").val(data.Code);
            $("#modal-editCourse-Duration").val(data.Duration);
            $("#modal-editCourse-Credit").val(data.Credit);
            $("#modal-editCourse-Outline").val(data.Outline);

            $.each(data.Trainers, (key, value) => {
                //console.log(value);
                $("#modal-editCourse-LeadTrainer").append(`<option value=${value.TrainerId}>${value.TrainerName}</option>`);
            });
        };

        $(document).on("click", ".js-editCourseModalPopup", (event) => {
            editCourseModal.modal("toggle");
            const rowData = getTableRowAsObject($(event.target).closest("tr"));
          
            setTimeout(() => {
                bindDataToEditCoursePopup(rowData);
            }, 100); // 
        });

        /** End**/

        // jQuery Code Ends Here
    });
})(jQuery);