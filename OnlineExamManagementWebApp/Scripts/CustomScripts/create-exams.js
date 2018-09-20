
$(function () {
    $("#js-examTypes").select2({
        placeholder: "Select a option",
        theme: "classic",
        selectOnClose: true
    });

    const courseId = $("#Id").val();
    let examList = new Array();

    getTableContents();

    autoSuggestSerial();
    
    function autoSuggestSerial() {
        let number = parseInt($("#create-exam-tableBody tr").length);
        $("#js-exam-serial").val(number + 1);
    }

    function getTableContents() {
        $("#create-exam-tableBody").find("tr").each(function () {
            let rowObject = {
                CourseId: courseId,
                SerialNo: parseInt($(this).find("td:eq(0)").text()),
                ExamType: $(this).find("td:eq(1)").text(),
                Topic: $(this).find("td:eq(2)").text(),
                Code: $(this).find("td:eq(3)").text(),
                Duration: parseInt($(this).find("input[name='Duration']").val()),
                FullMarks: $(this).find("td:eq(5)").text(),
                IsDeleted: false
            }
            rowObject.DurationHour = parseInt(rowObject.Duration / 60) ;
            rowObject.DurationMin = parseInt(rowObject.Duration % 60);
            examList.push(rowObject);
        });
    }

    function getTextBoxContents() {        
        let durationHour = parseInt($("#js-exam-duration-hour").val());
        let durationMin = parseInt($("#js-exam-duration-min").val());
        let duration = durationHour* 60 + durationMin;

        return {
            CourseId: courseId,
            SerialNo: parseInt($("#js-exam-serial").val()),
            ExamType: $("#js-examTypes").children("option").filter(":selected").text(),
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

    function reSequanceSerialNo(newSerial) {
        let arrayLength = parseInt($("#create-exam-tableBody tr").length) + 1;
        let arrayIndex = newSerial - 1;
        let loopCount = arrayLength - newSerial;

        for (let i = 0; i < loopCount; i++) {
            examList[arrayIndex].SerialNo += 1;
            arrayIndex++;
        }
    }

    function getCreateExamTableRow(addable) {
        let tableBody = `"
            <tr>
                <td>${addable.SerialNo}</td>
                <td>${addable.ExamType}</td>
                <td>${addable.Topic}</td>
                <td>${addable.Code}</td>
                <td>
                    <input type='hidden' name='Duration' value='${addable.Duration}' />
                    ${addable.DurationHour} : ${addable.DurationMin}
                 </td>
                <td>${addable.FullMarks}</td>
                <td>
                    <a href='#' class='js-createExam-ViewExam'>View | </a>
                    <a href='#' class='js-createExam-EditExam'>Edit | </a>
                    <a href='#' class='js-createExam-RemoveExam'>Remove</a>
                </td>
            "`;
        return tableBody;
    }

    function reBuildCreateExamTable() {
        let tableHtml = "";
        for (let i = 0; i < examList.length; i++) {            
            tableHtml += getCreateExamTableRow(examList[i]);
        }
        $("#create-exam-tableBody").html(tableHtml);
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

    // Add button functionality
    $("#js-btn-addExam").on("click", function () {
        let addable = getTextBoxContents();
               
        if (isNeedResequancing(addable.SerialNo)) {
            alert("Need resequancing");
            reSequanceSerialNo(addable.SerialNo);
        }
        
        examList.splice((addable.SerialNo - 1), 0, addable);
        
        reBuildCreateExamTable();
        autoSuggestSerial();

        console.log(examList);
    });


    // Finally save to database functionality
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
        });
    });


    // jquery ready function ends here;
});
