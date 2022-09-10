//IIFE
(() => {
    $(() => {
        /*
         * Initialization
         */
        const qoViewModal = $("#js-modal-viewQo");

        $("#js-modal-viewQo-qText").on("change", function () {
            $(this).height(0).height(this.scrollHeight);
        }).find("js-modal-viewQo-qText").trigger("change");
        // End


        const getRowOfQuestionTableAsObject = function (row) {
            let content = {
                QuestionId: row.find("a.btn.btn-primary.js-qoModalPopup").attr("data-question-id"),
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
        };

        const getOptionsByQuestionId = function (questionId) {
            return $.ajax({
                type: "GET",
                url: `/QuestionAnswer/GetOptionsByQuestionId?id=${questionId}`,
                dataType: "json"
            })
                .done(function (data) {

                });
        };


        // Popup Question Option Modal
        $(document).on("click", ".js-qoModalPopup", async function (event) {

            let tableRow = getRowOfQuestionTableAsObject($(event.target).closest("tr"));
            bindToViewQoModal(tableRow);

            try {
                let serverResponse = await getOptionsByQuestionId(tableRow.QuestionId);
                console.log(serverResponse);

            }

            catch (exception) {
                if (exception.status == 404) {
                    window.location = "/Error/Error404";
                }
            }

            //if (serverResponse.StatusCode == 200) {
            //    qoViewModal.modal("toggle");
            //}

        });

        // End
    });
})(jQuery);