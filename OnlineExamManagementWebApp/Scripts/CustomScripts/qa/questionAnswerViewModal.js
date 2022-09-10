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

        const getOptionsByQuestionId = async function (questionId) {
            try {
                await $.ajax({
                    type: "GET",
                    url: `/QuestionAnswer/GetOptionsByQuestionId?id=${questionId}`,
                    dataType: "json",
                    statusCode: {
                        404: function (response, statusText, jqXhr) {
                            console.log("response >>" + response);
                            console.log("statusText >>" + statusText);
                            console.log("statusText >>" + jqXhr);
                            window.location = "/Error/Error404";
                        }
                    }
                })
                    .done(function (data) {
                        bootbox.alert("ajax request successful");
                    })
                    .fail(function (data) {
                        bootbox.alert("ajax request failed");
                        console.log(data);
                    });

            } catch (e) {
                console.log(e.responseJSON.Message);
            }
        };


        // Popup Question Option Modal
        $(document).on("click", ".js-qoModalPopup", function (event) {


            setTimeout(function () {
                // Get data from HTML table
                let tableRow = getRowOfQuestionTableAsObject($(event.target).closest("tr"));
                bindToViewQoModal(tableRow);

                // Get data from server
                let optionsDataFromServer = getOptionsByQuestionId(tableRow.QuestionId);

                qoViewModal.modal("toggle");
            }, 300);
        });

        // End
    });
})(jQuery);