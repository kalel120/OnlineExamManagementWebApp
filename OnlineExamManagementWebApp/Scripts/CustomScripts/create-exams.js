
$(function () {
    var organizationId = $('#OrganizationId').val();
    var courseId = $('#Id').val();

    $("#js-examTypes").select2({
        placeholder: 'Select a option',
        theme: "classic",
        selectOnClose: true
    });

    function loadCreateExamTab() {
        $("js-organization").text($("#OrganizationCode").val());
        $("js-course").val($("#Name").val());
        $("js-exam-code").val($("#Code").val());
    }

    loadCreateExamTab();
});