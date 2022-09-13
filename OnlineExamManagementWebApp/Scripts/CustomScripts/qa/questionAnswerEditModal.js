// IIFE

(() => {
    $(() => {
        /**Initialization**/
        const qoEditModal = $("#js-modal-editQo");
        const qoEditModalForm = $("#js-modal-editQo-form");
        const qoEditOrderTextBox = $("#js-modal-editQo-order");
        const qoEditMarksTextBox = $("#js-modal-editQo-marks");
        const qoEditDescTextBox = $("#js-modal-editQo-qText");
        const qoEditSingleRadioBtn = $("#js-modal-editQo-oType-single");
        const qoEditMultiRadioBtn = $("#js-modal-editQo-oType-multiple");
        const optionsChkBoxDiv = $(".js-div-options-editModal");
        const editQoSubmitBtn = $("#js-modal-editQo-Submit");
        //editQoSubmitBtn.prop("disabled",true);
        /*END*/


        /** validation **/
        const validation = function() {
            return qoEditModalForm.validate({
                errorClass: "text-danger",
                errorElement: "div",

                rules: {
                    "Marks": {
                        required: true,
                        digits: true
                    },
                    "Description": {
                        required: true,
                        minlength: 10,
                        maxlength: 200 
                    }
                },
                messages: {
                    "Marks": {
                        required: "Insert Question Marks",
                        digits: "Only positive integer are allowed"
                    },
                    "Description": {
                        required: "Enter Question",
                        minlength: "Option can not be less than 10 character",
                        maxlength: "Option can not exceed more than 200 characters "
                    }
                },
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
                }
            }).form();
        };
        /*END*/

        /** Popup question options edit modal **/
        const getRowOfQuestionTableAsObject = function (row) {
            let content = {
                QuestionId: row.find("td:eq(5)").children(".js-qoEditModalPopup").attr("data-question-id"),
                Serial: $.trim(row.find("td:eq(0)").text()),
                Marks: $.trim(row.find("td:eq(1)").text()),
                Description: $.trim(row.find("td:eq(2)").text()),
                OptionType: $.trim(row.find("td:eq(3)").text())
            };

            return content;
        };

        const getOptionsByQuestionId = function (questionId) {
            return $.ajax({
                type: "GET",
                url: `/QuestionAnswer/GetOptionsByQuestionId?id=${questionId}`,
                dataType: "json"
            });
        };

        const bindToEditQoModal = function (data) {
            qoEditOrderTextBox.val(data.Serial);
            qoEditMarksTextBox.val(data.Marks);
            qoEditDescTextBox.val(data.Description);

            if (data.OptionType === qoEditSingleRadioBtn.val()) {
                qoEditSingleRadioBtn.prop("checked", true).iCheck("update");
                qoEditMultiRadioBtn.prop("checked", false).iCheck("update");
            }

            if (data.OptionType === qoEditMultiRadioBtn.val()) {
                qoEditMultiRadioBtn.prop("checked", true).iCheck("update");
                qoEditSingleRadioBtn.prop("checked", false).iCheck("update");
            }

            // bind options text to checkboxes
            optionsChkBoxDiv.find("span").each(function (index, element) {
                $(element).text(data.Options[index].Description);
            });

            // bind correct answers to checkboxes
            optionsChkBoxDiv.find("input[type=checkbox]").each(function (index, element) {
                if (data.Options[index].IsMarkedAsAnswer) {
                    $(element).prop("checked", true).iCheck("update");
                } else {
                    $(element).prop("checked", false).iCheck("update");
                }
            });

        };

        $(document).on("click", ".js-qoEditModalPopup", async function (event) {
            let tableRow = getRowOfQuestionTableAsObject($(event.target).closest("tr"));

            try {
                // asynchronously get options data from server
                tableRow.Options = await getOptionsByQuestionId(tableRow.QuestionId);
                bindToEditQoModal(tableRow);
                qoEditModal.modal("toggle");
            } catch (exception) {
                if (exception.status === 404) {
                    window.location = "/Error/Error404";
                } else {
                    console.log(exception);
                }
            }
        });
        /*END*/

        /** Quesiton Option Update, Save change button actions **/
        editQoSubmitBtn.on("click", function() {
            if (validation()) {
                bootbox.alert("validated");
            }
        });
        /*END*/
    });
})(jQuery);