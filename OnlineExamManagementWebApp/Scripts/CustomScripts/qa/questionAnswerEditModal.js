// IIFE

(() => {
    $(() => {
        /**Initialization**/
        const qoEditModal = $("#js-modal-editQo");
        const qoEditModalQuestionId = $("#js-editModal-hidden-qId");
        const qoEditModalForm = $("#js-modal-editQo-form");
        const qoEditModalFormInputs = $("#js-modal-editQo-form :input");
        const qoEditOrderTextBox = $("#js-modal-editQo-order");
        const qoEditMarksTextBox = $("#js-modal-editQo-marks");
        const qoEditDescTextBox = $("#js-modal-editQo-qText");
        const qoEditSingleRadioBtn = $("#js-modal-editQo-oType-single");
        const qoEditMultiRadioBtn = $("#js-modal-editQo-oType-multiple");
        const optionsChkBoxDiv = $(".js-div-options-editModal");
        const editQoSubmitBtn = $("#js-modal-editQo-Submit");
        const qoEditModalTbl = $("#js-tbl-editOptions-modal");
        const qoEditModalTblBody = $("#js-tbl-editOptionModal-tbody");
        const addOptionDiv = $(".js-div-addOption-editQoModal");


        const resetModalState = function () {
            editQoSubmitBtn.hide();
            addOptionDiv.hide();
        }

        resetModalState();
        /*END*/

        /*** Modal Reset*/
        qoEditModal.on("hidden.bs.modal", function () {
            $(this).find("form").trigger("reset");
            resetModalState();
        });

        /**END */

        /** On any input change, will trigger an event to enable disable save button */
        qoEditModalFormInputs.on("keyup change ifChanged", function () {
            editQoSubmitBtn.show();
        });

        qoEditModalTbl.on("change", function () {
            editQoSubmitBtn.show();
        });

        /**END */


        /** validation **/
        const validation = function () {
            return qoEditModalForm.validate({
                errorClass: "text-danger",
                errorElement: "div",

                rules: {
                    "Marks": {
                        required: true,
                        digits: true
                    },
                    "QuestionDescription": {
                        required: true,
                        minlength: 10,
                        maxlength: 200
                    },
                    "OptionDescription": {
                        required: true,
                        minlength: 1,
                        maxlength: 50
                    }
                },
                messages: {
                    "Marks": {
                        required: "Insert Question Marks",
                        digits: "Only positive integer are allowed"
                    },
                    "QuestionDescription": {
                        required: "Enter Question",
                        minlength: "Option can not be less than 10 character",
                        maxlength: "Option can not exceed more than 200 characters "
                    },
                    "OptionDescription": {
                        required: "Enter Option",
                        minlength: "Option can not be less than 1 character",
                        maxlength: "Option can not exceed more than 50 characters "
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

        const buildModalOptionsTable = function (data) {
            qoEditModalTblBody.empty();
            for (let index = 0; index < data.length; index++) {
                let html = `<tr>
                            <td>${data[index].Order}</td>
                            <td> ${data[index].Description}</td>`;

                if (data[index].IsMarkedAsAnswer) {
                    html += `<td><input type = "checkbox" name="OptionEditModalChkBox" checked="${data[index].IsMarkedAsAnswer}"/></td>`;
                } else {
                    html += `<td><input type = "checkbox" name="OptionEditModalChkBox"/></td>`;
                }
                html += `<td><a href="#" data-option-id="${data[index].OptionId}" class="js-editOptions-modal-remove-option btn btn-danger"><i class="avoid-clicks fa fa-trash-o"> Remove </i> </a> </td>
                         </tr>`;


                qoEditModalTbl.append(html);
            }
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
            //optionsChkBoxDiv.find("span").each(function (index, element) {
            //    $(element).text(data.Options[index].Description);
            //});

            // bind correct answers to checkboxes
            //optionsChkBoxDiv.find("input[type=checkbox]").each(function (index, element) {
            //    if (data.Options[index].IsMarkedAsAnswer) {
            //        $(element).prop("checked", true).iCheck("update");
            //    } else {
            //        $(element).prop("checked", false).iCheck("update");
            //    }
            //});
        };



        $(document).on("click", ".js-qoEditModalPopup", async function (event) {
            try {
                let tableRow = getRowOfQuestionTableAsObject($(event.target).closest("tr"));
                bindToEditQoModal(tableRow);
                qoEditModalQuestionId.val(tableRow.QuestionId);

                // asynchronously get options data from server
                let options = await getOptionsByQuestionId(tableRow.QuestionId);

                // dynamically build html options table
                buildModalOptionsTable(options);

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

        /*** Remove Option from edit modal and db*/
        const removeOptionFromDb = function (optionId, examId) {
            let optionToRemove = {
                OptionId: optionId,
                ExamId: examId
            };

            return $.ajax({
                type: "PUT",
                url: `/QuestionAnswer/RemoveAnOption`,
                dataType: "json",
                data: optionToRemove
            })
                .fail(function (res, textStatus, errorThrown) {
                    console.log(res);
                    console.log(textStatus);
                    console.log(errorThrown);
                });
        };


        const reSequenceOptionTbl = function (options) {
            return $.ajax({
                type: "PUT",
                url: `/QuestionAnswer/ReOrderOptionsOnRemove`,
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(options)
            })
                .fail(function (res, textStatus, errorThrown) {
                    console.log(res);
                    console.log(textStatus);
                    console.log(errorThrown);
                });
        };

        const removeRowOfOptionsTbl = async function (row) {
            let optionIdToRemove = row.find("td:eq(3) > a").attr("data-option-id");
            let examId = $("#js_examId").val();
            let currentOptions = new Array();

            // remove from html table
            row.remove();


            // fetch html table content as object
            qoEditModalTblBody.find("tr").each(function (index, element) {
                let option = {
                    Order: index + 1,
                    OptionId: $(element).find("td:eq(3) > a").attr("data-option-id"),
                    QuestionId: qoEditModalQuestionId.val(),
                    ExamId: examId
                };
                currentOptions.push(option);
            });

            // remove from  server and re-sequence serial
            try {
                if (await removeOptionFromDb(optionIdToRemove, examId)) {
                    // re-sequence option serial and build options table
                    if (await reSequenceOptionTbl(currentOptions)) {
                        buildModalOptionsTable(await getOptionsByQuestionId(qoEditModalQuestionId.val()));
                    }
                }

            }
            catch (exception) {
                if (exception.status === 404) {
                     window.location = "/Error/Error404";
                } else {
                    console.log(exception);
                }
            }
        };

        $(document).on("click", ".js-editOptions-modal-remove-option", function (event) {
            bootbox.confirm("Are you sure?", function (result) {
                if (result) {
                    removeRowOfOptionsTbl($(event.target).closest("tr"));
                    addOptionDiv.show();
                }
            });
        });
        /*** END */


        // For testing purpose below function fetches data from server and build options table. Need to refactor this later.
        $(document).on("click", "#js-btn-editOptionModal-AddOption", async function (event) {
            // asynchronously get options data from server and dynamically build html options table
            buildModalOptionsTable(await getOptionsByQuestionId(qoEditModalQuestionId.val()));
        });

        /** Quesiton Option Update, Save change button actions **/
        editQoSubmitBtn.on("click", function () {
            if (validation()) {
                bootbox.alert("validated");
            }
        });
        /*END*/
    });
})(jQuery);