//IIFE
(() => {
    $(() => {
        /*
         * Initialization
         */
        const qoViewModal = $("#js-modal-viewQo");
        const optionsChkBoxDiv = $(".js-div-options-viewModal");
        /*END*/

        const getRowOfQuestionTableAsObject = function (row) {
            let content = {
                QuestionId: row.find("a.btn.btn-primary.js-qoViewModalPopup").attr("data-question-id"),
                Serial: $.trim(row.find("td:eq(0)").text()),
                Marks: $.trim(row.find("td:eq(1)").text()),
                Description: $.trim(row.find("td:eq(2)").text()),
                OptionType: $.trim(row.find("td:eq(3)").text())
            };

            return content;
        };

        const bindToViewQoModal = function (data) {
            $("#js-modal-viewQo-order").val(data.Serial);
            $("#js-modal-viewQo-marks").val(data.Marks);
            $("#js-modal-viewQo-qText").val(data.Description);
            $("#js-modal-viewQo-optionType").val(data.OptionType);

            // bind options text to checkboxes
            optionsChkBoxDiv.find("span").each(function (index, element) {
                $(element).text(data.Options[index].Description);
            });

            // bind correct answers to checkboxes
            optionsChkBoxDiv.find("input[type=checkbox]").each(function (index, element) {
                if (data.Options[index].IsMarkedAsAnswer) {
                    $(element).removeAttr("disabled").prop("checked", true).iCheck("update");
                } else {
                    $(element).prop("disabled",true).prop("checked", false).iCheck("update");
                }
            });
        };

        const getOptionsByQuestionId = function (questionId) {
            return $.ajax({
                type: "GET",
                url: `/QuestionAnswer/GetOptionsByQuestionId?id=${questionId}`,
                dataType: "json"
            });
        };


        /** Popup question options view modal **/
        $(document).on("click", ".js-qoViewModalPopup", async function (event) {
            let tableRow = getRowOfQuestionTableAsObject($(event.target).closest("tr"));

            try {
                tableRow.Options = await getOptionsByQuestionId(tableRow.QuestionId);
                bindToViewQoModal(tableRow);
                qoViewModal.modal("toggle");
            }

            catch (exception) {
                if (exception.status === 404) {
                    window.location = "/Error/Error404";
                } else {
                    console.log(exception);
                }
            }
        });

        /*END*/
    });
})(jQuery);