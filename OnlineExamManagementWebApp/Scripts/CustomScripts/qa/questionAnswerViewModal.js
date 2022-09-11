//IIFE
(() => {
    $(() => {
        /*
         * Initialization
         */
        const qoViewModal = $("#js-modal-viewQo");
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
            const firstOptionChk = $("#js-modal-viewQo-firstOption");
            const firstOptionSpan = $("#js-span-modal-viewQo-firstOption");
            const secondOptionChk = $("#js-modal-viewQo-secondOption");
            const secondOptionSpan = $("#js-span-modal-viewQo-secondOption");
            const thirdOptionChk = $("#js-modal-viewQo-thirdOption");
            const thirdOptionSpan = $("#js-span-modal-viewQo-thirdOption");
            const fourthOptionChk = $("#js-modal-viewQo-fourthOption");
            const fourthOptionSpan = $("#js-span-viewQo-fourthOption");


            $("#js-modal-viewQo-order").val(data.Serial);
            $("#js-modal-viewQo-marks").val(data.Marks);
            $("#js-modal-viewQo-qText").val(data.Description);
            $("#js-modal-viewQo-optionType").val(data.OptionType);

            firstOptionChk.text(data.Options[0].Description);
            firstOptionSpan.text(data.Options[0].Description);

            if (data.Options[0].IsMarkedAsAnswer) {
                firstOptionChk.removeAttr("disabled").prop("checked", true).iCheck("update");
            } 

            secondOptionChk.text(data.Options[1].Description);
            secondOptionSpan.text(data.Options[1].Description);

            if (data.Options[1].IsMarkedAsAnswer) {
                secondOptionChk.removeAttr("disabled").prop("checked", true).iCheck("update");
            } 

            thirdOptionChk.text(data.Options[2].Description);
            thirdOptionSpan.text(data.Options[2].Description);


            if (data.Options[2].IsMarkedAsAnswer) {
                thirdOptionChk.removeAttr("disabled").prop("checked", true).iCheck("update");
            }

            fourthOptionChk.text(data.Options[3].Description);
            fourthOptionSpan.text(data.Options[3].Description);

            if (data.Options[3].IsMarkedAsAnswer) {
                fourthOptionChk.removeAttr("disabled").prop("checked", true).iCheck("update");
            }
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