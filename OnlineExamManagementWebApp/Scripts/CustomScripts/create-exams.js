
$(function () {
    $("#js-examTypes").select2({
        placeholder: "--SELECT EXAM TYPE--",
        theme: "classic",
        selectOnClose: true
    });

    /** Initialization **/

    const courseId = $("#Id").val();
    let examList = new Array();

    getTableCellsToObjects();
    autoSuggestSerial();

    /** Initialization END**/    

    function autoSuggestSerial() {
        let number = parseInt($("#create-exam-tableBody tr").length);
        $("#js-exam-serial").val(number + 1);
    }

    function getTableCellsToObjects() {
        $("#create-exam-tableBody").find("tr").each(function () {
            let rowObject = {
                CourseId: courseId,
                SerialNo: parseInt($(this).find("td:eq(0)").text()),
                Type: $(this).find("td:eq(1)").text(),
                Topic: $(this).find("td:eq(2)").text(),
                Code: $(this).find("td:eq(3)").text(),
                Duration: parseInt($(this).find("input[name='Duration']").val()),
                FullMarks: $(this).find("td:eq(5)").text(),
                IsDeleted: false
            }
            rowObject.DurationHour = parseInt(rowObject.Duration / 60);
            rowObject.DurationMin = parseInt(rowObject.Duration % 60);
            examList.push(rowObject);
        });
    }

    function getTextBoxContentsToObject() {
        let durationHour = parseInt($("#js-exam-duration-hour").val());
        let durationMin = parseInt($("#js-exam-duration-min").val());
        let duration = durationHour * 60 + durationMin;

        return {
            CourseId: courseId,
            SerialNo: parseInt($("#js-exam-serial").val()),
            Type: $("#js-examTypes").children("option").filter(":selected").text(),
            Topic: $("#js-exam-topic").val(),
            Code: $("#js-exam-code").val(),
            DurationHour: durationHour,
            DurationMin: durationMin,
            Duration: duration,
            FullMarks: $("#js-exam-full-marks").val(),
            IsDeleted: false
        };
    }

    function isNeedResequancing(serialNo) {
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
                    <a href='#' class='js-createExam-ViewExam'>View | </a>
                    <a href='#' class='js-createExam-EditExam'>Edit | </a>
                    <a href='#' class='js-createExam-RemoveExam'>Remove</a>
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

    function reLoadCreateExamTableFromDb() {
        $("#create-exam-tableBody").html("");

        $.ajax({
            url: "/Course/GetActiveExamByCoureId?courseId=" + courseId,
            type: "GET",
            contentType: "application/json",
            success: function (result) {
                let tableHtml = "";

                $.each(result, function (key, item) {
                    tableHtml += getCreateExamTableRow(item);
                });

                $("#create-exam-tableBody").html(tableHtml);
            },
            error: function (message) {
                console.log(message.responseText);
            }
        });
    }

    function removeExamFromDb(examCode, courseId) {
        $.post("/Course/RemoveExamByCode", { Code: examCode, CourseId: courseId })
            .done(function () {
                alert(`${examCode} is removed`);
            });
    }

    function isDuplicateExamCode(examCode) {
        var isDuplicate = false;

        $("#create-exam-tableBody").find("tr").each(function () {
            let tdText = $(this).find("td:eq(3)").text().trim();
            if (tdText === examCode) {
                isDuplicate = true;
            }
            if (isDuplicate) {
                return;
            }
        });
        return isDuplicate;
    }

    // Check Serial number textbox validity
    $("#js-exam-serial").on("change", function () {
        let textBox = $("#js-exam-serial");
        if (!$.isNumeric(textBox.val())) {
            alert("Invalid serial");
            autoSuggestSerial();
        }

        let serialNo = parseInt(textBox.val());
        if (serialNo <= 0) {
            alert("Invalid serial");
            autoSuggestSerial();
        }

        let tableLength = parseInt($("#create-exam-tableBody tr").length);

        if (serialNo > (tableLength + 1)) {
            alert("Invalid serial");
            autoSuggestSerial();
        }
    });

    // Remove cell item functionality
    $(document).on("click", ".js-createExam-RemoveExam", function () {
        if (!confirm("Are you sure you want to Remove")) {
            return;
        }
        let closestRow = $(this).closest("tr");
        let examCode = closestRow.find("td:eq(3)").text().trim();
        let rowSerial = parseInt(closestRow.find("td:eq(0)").text());


        examList = examList.filter(s => s.SerialNo !== rowSerial);
        removeExamFromDb(examCode, courseId);
        reSequanceSerialNo();
        reBuildCreateExamTable();
    });

    // Add button functionality
    $("#js-btn-addExam").on("click", function () {
        if ($("#js-examTypes").val() === "0") {
            alert("Select a type");
            return;
        }

        let addable = getTextBoxContentsToObject();

        if (isDuplicateExamCode(addable.Code)) {
            alert("Exam Code already exists");
            return;
        }

        if (isNeedResequancing(addable.SerialNo)) {
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
        $.ajax({
            url: "/Course/SaveAllExams",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: JSON.stringify(examList),

            success: function (result) {
                if (result) {
                    alert("Saved");
                }
            },

            error: function (message) {
                console.log(message.responseText);
            }
        }).done(function () {
            reLoadCreateExamTableFromDb();
        });
    });


    // jquery ready function ends here;
});
