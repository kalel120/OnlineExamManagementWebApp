
$(function () {
    $("#js-examTypes").select2({
        placeholder: 'Select a option',
        theme: "classic",
        selectOnClose: true
    });




    $(document).on("click", "#js-btn-addExam", function () {


        //let organizationId = $("#OrganizationId").val();
        //let courseId = $("#Id").val();
        //let examCode = $("#js-exam-code").val();
        //let examTopic = $("#js-exam-topic").val();
        //let fullMarks = $("#js-exam-full-marks").val();
        //let durationHour = $("#js-exam-duration-hour").val();
        //let durationMin = $("#js-exam-duration-min").val();
        //let serial = $("#js-exam-serial").val();
        //let selectedExamType = $("#js-examTypes").children("option").filter(":selected").text();


        // table builder
        //    //<td><input type='hidden' name='ExamSerial' value='${serial}' />

        //    let tableBody = `"
        //    <tr>            
        //        <td>${selectedExamType}</td>
        //        <td>${examTopic}</td>
        //        <td>${examCode}</td>
        //        <td>${(durationHour * 60) + durationMin}</td>
        //        <td>${fullMarks}</td>
        //        <td>
        //            <a href='#' class="js-createExam-ViewExam">View | </a>
        //            <a href='#' class="js-createExam-EditExam">Edit | </a>
        //            <a href='#' class="js-createExam-RemoveExam">Remove</a>
        //        </td>            
        //"`;
        //    console.log(tableBody);
        //    $("#create-exam-tableBody").append(tableBody);
        //});


    });

    // jquery ready function ends here;

});