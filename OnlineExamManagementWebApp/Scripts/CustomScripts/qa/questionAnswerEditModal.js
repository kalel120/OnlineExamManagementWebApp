// IIFE

(() => {
    $(() => {
        /**Initialization**/
        const qoEditModal = $("#js-modal-editQo");

        const qoEditOrderTextBox = $("#js-modal-editQo-order");
        const qoEditMarksTextBox = $("#js-modal-editQo-marks");
        const qoEditDescTextBox = $("#js-modal-editQo-qText");
        const qoEditSingleRadioBtn = $("#js-modal-editQo-oType-single");
        const qoEditMultiRadioBtn = $("#js-modal-editQo-oType-multiple");

        const optionsChkBoxDiv = $(".js-div-options-editModal");

        /*END*/

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

            optionsChkBoxDiv.find("span").each(function (index, element) {
                $(element).text(data.Options[index].Description);
            });

            optionsChkBoxDiv.find("input[type=checkbox]").each(function (index, element) {
                if (data.Options[index].IsMarkedAsAnswer) {
                    $(element).prop("checked", true).iCheck("update");
                } else {
                    $(element).prop("checked", false).iCheck("update");
                }
            });

        };

        /** Popup question options edit modal **/
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
    });
})(jQuery);