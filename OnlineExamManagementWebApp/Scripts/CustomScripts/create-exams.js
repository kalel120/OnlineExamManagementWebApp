
$(function () {
    $("#js-examTypes").select2({
        placeholder: 'Select a option',
        theme: "classic",
        selectOnClose: true
    });

    let examsToSave = new Array();

    const courseId = $("#Id").val();

    function getTextBoxContents() {
        let txtBoxValues = {
            CourseId: courseId,
            SerialNo: $("#js-exam-serial").val(),
            ExamType: $("#js-examTypes").children("option").filter(":selected").text(),
            Topic: $("#js-exam-topic").val(),
            Code: $("#js-exam-code").val(),
            DurationHour: $("#js-exam-duration-hour").val(),
            DurationMin: $("#js-exam-duration-min").val(),
            FullMarks: $("#js-exam-full-marks").val(),
            IsDeleted: false
        };
        return txtBoxValues;
    }


    $(document).on("click", "#js-btn-addExam", function () {

        let addable = getTextBoxContents();
        examsToSave.push(addable);

        // table builder       
        let tableBody = `"
            <tr>
                <td>${addable.SerialNo}</td>
                <td>${addable.ExamType}</td>
                <td>${addable.Topic}</td>
                <td>${addable.Code}</td>
                <td>${addable.DurationHour} hh : ${addable.DurationMin} mm</td>
                <td>${addable.FullMarks}</td>
                <td>
                    <a href='#' class='js-createExam-ViewExam'>View | </a>
                    <a href='#' class='js-createExam-EditExam'>Edit | </a>
                    <a href='#' class='js-createExam-RemoveExam'>Remove</a>
                </td>            
            "`;
        console.log(tableBody);
        $("#create-exam-tableBody").append(tableBody);
    });


    // Finally save to database functionality
    $("#js-btn-SaveAllExam").on("click", function () {

        $.ajax({
            url: "/Course/SaveAllExams",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: JSON.stringify(examsToSave),

            success: function(result) {
                if (result) {
                    alert("Saved");
                }
            },

            error: function(message) {
                console.log(message.responseText);
            }
        });
    });


    // jquery ready function ends here;
});
