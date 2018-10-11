
$(function () {
    /** Initialization **/
    $("#js-examTypes").select2();

    const courseId = $("#Id").val();
    let examList = [];

    getTableCellAsToObjects();
    autoSuggestSerial();
    /** Initialization END**/

    const createExamValidation = () => {
        $.validator.addMethod("validSerial", (value) => {
            if (value > 0 && value < ($("#create-exam-tableBody tr").length + 2)) {
                return true;
            }
            return false;
        }, "Must be greater than 0 next incremntal serial number");

        $.validator.addMethod("uniqueExamCode", (value) => {
            var isUnique = true;

            $("#create-exam-tableBody").find("tr").each(function () {
                let tdText = $(this).find("td:eq(3)").text().trim();
                if (tdText === value) {
                    isUnique = false;
                    return;
                }
            });
            return isUnique;
        },
            "Exam code already exists");

       const validator = $("#form-createExam").validate({
            errorClass: "text-danger",
            errorElement: "div",

            errorPlacement: function (error, element) {
                if (element.parent(".input-group > div").length) {
                    element.parent(".input-group > div").append(error);
                }
                element.parent(".form-group > div").append(error);
            },

            highlight: function (element) {
                $(element).closest(".form-group").removeClass("has-success has-error")
                    .addClass("has-error");
            },
            success: function (element) {
                element.closest(".form-group").removeClass("has-success has-error");
            },

            rules: {
                "Code": {
                    required: true,
                    uniqueExamCode: true
                },
                "js-examTypes": {
                    required: true
                },
                "Topic": {
                    required: true
                },
                "FullMarks": {
                    required: true,
                    digits: true
                },
                "DurationHour": {
                    required: true,
                    digits: true,
                    range: [0, 24]
                },
                "DurationMin": {
                    required: true,
                    digits: true,
                    range: [0,59]
                },
                "SerialNo": {
                    required: true,
                    digits: true,
                    validSerial: true
                }
            },
            messages: {
                "Code": {
                    required: "Exam Code is required",
                    uniqueExamCode: "Exam code already exists"
                },
                "js-examTypes": {
                    required: "Select type"
                },
                "Topic": {
                    required: "Topic required"
                },
                "FullMarks": {
                    required: "Marks required",
                    digits: "Only numbers are allowed"
                },
                "DurationHour": {
                    required: "Hour required",
                    digits: "Only numbers are allowed",
                    range: "Hours must be between 0 to 24"
                },
                "DurationMin": {
                    required: "Min required",
                    digits: "Only numbers  are allowed",
                    range: "Minutes must be between 0 to 59"
                },
                "SerialNo": {
                    required: "This is required",
                    digits: "Only integer and greater than 0 is allowed"
                }
            }
        }).form();

        $("#js-examTypes").on("change", function () {
            $(this).valid();
        });

        return validator;
    }

    const editExamValiation = () => {
        const modalForm = $("#_form-modal-editExam");

        return modalForm.validate({
            errorClass: "text-danger",
            errorElement: "div",

            errorPlacement: function (error, element) {
                if (element.parent(".input-group > div").length) {
                    element.parent(".input-group > div").append(error);
                }
                element.parent(".form-group > div").append(error);
            },

            highlight: function (element) {
                $(element).closest(".form-group").removeClass("has-success has-error")
                    .addClass("has-error");
            },

            unhighlight: function (element) {
                $(element).closest(".form-group").removeClass("has-error");
            },

            success: function (element) {
                element.closest(".form-group").removeClass("has-success has-error");
            },

            rules: {
                "js-modal-editExam-Code": {
                    required: true
                },
                "js-modal-editExam-Type": {
                    required: true
                },
                "js-modal-editExam-Topic": {
                    required: true
                },
                "js-modal-editExam-FullMarks": {
                    required: true,
                    digits: true
                },
                "js-modal-editExam-DurationHour": {
                    required: true,
                    digits: true,
                    range: [0, 24]
                },
                "js-modal-editExam-DurationMin": {
                    required: true,
                    digits: true,
                    range: [0, 59]
                }
            },

            messages: {
                "js-modal-editExam-Code": {
                    required: "Exam Code is required"
                },
                "js-modal-editExam-Type": {
                    required: "Select exam"
                },
                "js-modal-editExam-Topic": {
                    required: "This is required"
                },
                "js-modal-editExam-FullMarks": {
                    required: "This is required",
                    digits: "Only numbers  are allowed"
                },
                "js-modal-editExam-DurationHour": {
                    required: "This is required",
                    digits: "Only numbers  are allowed",
                    range: "Min must be between 0 to 24"
                },
                "js-modal-editExam-DurationMin": {
                    required: "This is required",
                    digits: "Only numbers  are allowed",
                    range: "Min must be between 0 to 59"
                }
            }
        }).form();
    }

    function autoSuggestSerial() {
        let number = parseInt($("#create-exam-tableBody tr").length);
        $("#js-exam-serial").val(number + 1);
    }

    function getTableRowAsObject(tableRow) {
        let rowObject = {
            CourseId: courseId,
            SerialNo: parseInt(tableRow.find("td:eq(0)").text()),
            Type: tableRow.find("td:eq(1)").text(),
            Topic: tableRow.find("td:eq(2)").text(),
            Code: tableRow.find("td:eq(3)").text(),
            Duration: parseInt(tableRow.find("input[name='Duration']").val()),
            FullMarks: tableRow.find("td:eq(5)").text(),
            IsDeleted: false
        }
        rowObject.DurationHour = parseInt(rowObject.Duration / 60);
        rowObject.DurationMin = parseInt(rowObject.Duration % 60);

        return rowObject;
    };

    function getTableCellAsToObjects() {
        $("#create-exam-tableBody").find("tr").each(function () {
            examList.push(getTableRowAsObject($(this)));
        });
    }

    function getTextBoxContentsAsObject() {
        let durationHour = parseInt($("#js-exam-duration-hour").val());
        let durationMin = parseInt($("#js-exam-duration-min").val());
        let duration = durationHour * 60 + durationMin;

        return {
            CourseId: courseId,
            SerialNo: parseInt($("#js-exam-serial").val()),
            Type: $("#js-examTypes").children("option").filter(":selected").text(),
            Topic: $("#js-exam-topic").val(),
            Code: $("#js-exam-code").val(),
            Duration: duration,
            FullMarks: $("#js-exam-full-marks").val(),
            IsDeleted: false,
            DurationHour: durationHour,
            DurationMin: durationMin
        };
    }

    function isNeedReSequancingAfterAdd(serialNo) {
        let result = false;
        for (let index = 0; index < examList.length; index++) {
            if (serialNo === examList[index].SerialNo)
                result = true;
        }
        return result;
    }

    function reSequanceSerialNo() {
        for (let index = 0; index < examList.length; index++) {
            examList[index].SerialNo = index + 1;
        }
    }

    function getCreateExamTableRow(addable) {
        return `"
            <tr>
                <td>${addable.SerialNo}</td>
                <td>${addable.Type}</td>
                <td>${addable.Topic}</td>
                <td>${addable.Code}</td>
                <td>
                    <input type='hidden' name='Duration' value='${addable.Duration}' />
                    ${addable.DurationHour} hh : ${addable.DurationMin} mm
                 </td>
                <td>${addable.FullMarks}</td>
                <td>
                     <a href='#' class='js-createExam-ViewExamLink'>View | </a>
                     <a href='#' class='js-createExam-EditExamLink'>Edit | </a>
                     <a href='#' class='js-createExam-RemoveExamLink'>Remove</a>
                </td>
            "`;
    }

    function reBuildCreateExamTable() {
        let tableHtml = "";
        for (let i = 0; i < examList.length; i++) {
            tableHtml += getCreateExamTableRow(examList[i]);
        }
        $("#create-exam-tableBody").html(tableHtml);
        autoSuggestSerial();
    }

    function removeExamFromDb(examCode, courseId, position) {
        if (position) {
            $.post("/Course/RemoveExamByCode", { Code: examCode, CourseId: courseId })
                .done(function () {
                    saveAllExams(examList);
                });
        } else {
            $.post("/Course/RemoveExamByCode", { Code: examCode, CourseId: courseId })
                .done(function () {
                    alert(`${examCode} is removed`);
                });
        }
    }

    function saveAllExams(exams) {
        $.ajax({
            url: "/Course/SaveAllExams",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: JSON.stringify(exams),

            success: function (result) {
                if (result) {
                    alert("Saved");
                }
            },

            error: function (message) {
                console.log(message.responseText);
            }
        });
    }

    // Add cell item functionality
    $("#js-btn-addExam").on("click", function () {
        if (!createExamValidation()) {
            return;
        }
        const addable = getTextBoxContentsAsObject();

        if (isNeedReSequancingAfterAdd(addable.SerialNo)) {
            alert("Exam serial number will be changed");
            examList.splice((addable.SerialNo - 1), 0, addable);
            reSequanceSerialNo();
        } else {
            examList.push(addable);
        }

        reBuildCreateExamTable();
    });

    // Save all to database functionality
    $("#js-btn-SaveAllExam").on("click", function () {
        saveAllExams(examList);
    });

    // Remove cell item functionality
    $(document).on("click", ".js-createExam-RemoveExamLink", function () {
        let closestRow = $(this).closest("tr");
        let examCode = closestRow.find("td:eq(3)").text().trim();
        let rowSerial = parseInt(closestRow.find("td:eq(0)").text());

        if (rowSerial === examList.length && confirm("Are you sure?")) {
            examList = examList.filter(s => s.SerialNo !== rowSerial);
            reBuildCreateExamTable();
            removeExamFromDb(examCode, courseId);
        }
        else if (confirm("serial number will be resequanced.\nAre you Sure?")) {
            examList = examList.filter(s => s.SerialNo !== rowSerial);
            reSequanceSerialNo();
            reBuildCreateExamTable();

            removeExamFromDb(examCode, courseId, rowSerial);
        }
    });

    /************* View Exam Modal Popup functionality *******************/
    const bindDataToViewExamModal = (data)=> {
        $("#js-modal-viewExam-OrgName").val($("#js-organization").val());
        $("#js-modal-viewExam-CourseName").val($("#js-course-code").val());
        $("#js-modal-viewExam-Serial").val(data.SerialNo);
        $("#js-modal-viewExam-Type").val(data.Type);
        $("#js-modal-viewExam-Topic").val(data.Topic);
        $("#js-modal-viewExam-Code").val(data.Code);
        $("#js-modal-viewExam-DurationHour").val(data.DurationHour);
        $("#js-modal-viewExam-DurationMin").val(data.DurationMin);
        $("#js-modal-viewExam-FullMarks").val(data.FullMarks);
    };

    $(document).on("click", ".js-createExam-ViewExamLink", function (event) {
        $("#js-modal-viewExam").modal("toggle");
        let dataForPopup = getTableRowAsObject($(event.target).closest("tr"));
        bindDataToViewExamModal(dataForPopup);
    });
    /************* END *******************/

    /************* Edit Exam Modal Popup functionality *******************/
    const editExamModal = $("#js-modal-editExam");
   
    const getEditExamModalContents = ()=> {
        const durationHour = parseInt($("#js-modal-editExam-DurationHour").val());
        const durationMin = parseInt($("#js-modal-editExam-DurationMin").val());
        const duration = durationHour * 60 + durationMin;

        return {
            CourseId: courseId,
            SerialNo: parseInt($("#js-modal-editExam-Serial").val()),
            Type: $("#js-modal-editExam-Type").children("option").filter(":selected").text(),
            Topic: $("#js-modal-editExam-Topic").val(),
            Code: $("#js-modal-editExam-Code").val(),
            Duration: duration,
            FullMarks: $("#js-modal-editExam-FullMarks").val(),
            IsDeleted: false,
            DurationHour: durationHour,
            DurationMin: durationMin
        };
    };

    const bindDataToEditExamPopup =(data)=> {
        $("#js-modal-editExam-Index").val(data.SerialNo - 1);
        $("#js-modal-editExam-OrgName").val($("#js-organization").val());
        $("#js-modal-editExam-CourseName").val($("#js-course-code").val());

        $("#js-modal-editExam-Serial").val(data.SerialNo);
        $("#js-modal-editExam-Type option").each(function () {
            if ($(this).text() === data.Type) {
                $(this).prop("selected", true);
            }
        });
        $("#js-modal-editExam-Topic").val(data.Topic);
        $("#js-modal-editExam-Code").val(data.Code);
        $("#js-modal-editExam-Duration").val(data.Duration);
        $("#js-modal-editExam-DurationHour").val(data.DurationHour);
        $("#js-modal-editExam-DurationMin").val(data.DurationMin);
        $("#js-modal-editExam-FullMarks").val(data.FullMarks);
    };

    $(document).on("click", ".js-createExam-EditExamLink", function (event) {
        editExamModal.modal("toggle");
        const dataForPopup = getTableRowAsObject($(event.target).closest("tr"));
        bindDataToEditExamPopup(dataForPopup);
    });

    const editExam = (editable) => {
        const arrayIndex = $("#js-modal-editExam-Index").val();
        const existingCode = examList[arrayIndex].Code;
        examList.splice(arrayIndex, 1, editable);

        $.post("/Course/UpdateExamByCode", { existingCode: existingCode, dto: editable })
            .done(() => alert("Updated"))
            .fail(() => alert("Something went wrong"));
    };

    const isExamEditable = (editable) => {
        let examCodes = [];

        $("#create-exam-tableBody").find("tr").each(function () {
            if (editable.SerialNo != $(this).find("td:eq(0)").text()) {
                examCodes.push($(this).find("td:eq(3)").text().trim().toLowerCase());
            }
        });

        if (examCodes.includes(editable.Code.toLowerCase())) {
            return false;
        }
        return true;
    };

    const isExamExists = (examCode) => {
        return new Promise((resolve, reject) => {
            $.post("/Course/IsExamExists", { courseId: courseId, examCode: examCode })
                .done((data) => resolve(data));
        });
    };

    $("#js-modal-btn-updateExam").on("click", function () {
        if (!editExamValiation()) {
            return false;
        }
        
        let editable = getEditExamModalContents();

        if (!isExamEditable(editable)) {
            alert("Code already exists");
            return false;
        }

        isExamExists(editable.Code).then((result) => {
            if (result) {
                editExam(editable);
                reBuildCreateExamTable();
            } else {
                examList.splice($("#js-modal-editExam-Index").val(), 1, editable);
                reBuildCreateExamTable();
                alert("Updated. Still need to save to db");
            }
        });

        editExamModal.modal("toggle");
        return true;
    });

    $("#js-modal-close-updateExam").on("click", function() {
        $("#_form-modal-editExam").validate().resetForm();
    });
    /************* END *******************/

    // jQuery ready function ends here;
});