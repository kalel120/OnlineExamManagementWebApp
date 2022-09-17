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

        /** Jobs when modal pops */

        const setCheckBoxForSingleAnswer = function () {
            $(document).on("click", "input[name='OptionEditModalChkBox']", function () {
                if (!qoEditSingleRadioBtn.is(":checked")) { return; }

                $("input[name='OptionEditModalChkBox']").not(this).prop("checked", false);
            });
        };

        // when modal pops, set behavior of checkbox based on option type selected
        qoEditModal.on("shown.bs.modal", function () {
            if (!qoEditSingleRadioBtn.is(":checked")) { return; }

            setCheckBoxForSingleAnswer();
        });


        // if single answer type is selected, make checkbox behave like radio button
        $(document).on("ifClicked", "#js-modal-editQo-oType-single", function () {
            setCheckBoxForSingleAnswer();
        });

        // if multiple answer is selected, then reset checkbox behavior
        $(document).on("ifClicked", "#js-modal-editQo-oType-multiple", function () {
            $("input[name='OptionEditModalChkBox']").each(function (index, item) {
                if ($(this).is(":checked")) { return; }

                $(this).prop("checked", false);
            });
        });
        /**END*/

        /** Jobs when modal got hidden */
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
            if (!data) { return; }

            qoEditModalTblBody.empty();

            //build html and append to tablebody
            for (let index = 0; index < data.length; index++) {
                let html = `<tr>
                            <td>${data[index].Order}</td>
                            <td> ${data[index].Description}</td>`;

                if (data[index].IsMarkedAsAnswer) {
                    html += `<td><input type = "checkbox" value=${data[index].OptionId} name="OptionEditModalChkBox" checked="${data[index].IsMarkedAsAnswer}"/></td>`;
                } else {
                    html += `<td><input type = "checkbox" value=${data[index].OptionId} name="OptionEditModalChkBox"/></td>`;
                }
                html += `<td><a href="#" data-option-id="${data[index].OptionId}" class="js-editOptions-modal-remove-option btn btn-danger"><i class="avoid-clicks fa fa-trash-o"> Remove </i> </a> </td>
                         </tr>`;

                qoEditModalTbl.append(html);
            }

            // hide add option button based on table row count
            if (data.length === 4 && addOptionDiv.is(":visible")) {
                addOptionDiv.hide();
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


        const reSequenceOptionTbl = function (options, examId, questionId) {
            return $.ajax({
                type: "PUT",
                url: `/QuestionAnswer/ReOrderOptionsOnRemove?examId=${examId}&questionId=${questionId}`,
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
            let examId = $("#js_examId").val();
            let questionId = qoEditModalQuestionId.val();
            let optionIdToRemove = row.find("td:eq(3) > a").attr("data-option-id");

            let currentOptions = new Array();

            // remove from html table
            row.remove();

            // fetch html table content as object
            qoEditModalTblBody.find("tr").each(function (index, element) {
                let option = {
                    Order: index + 1,
                    OptionId: $(element).find("td:eq(3) > a").attr("data-option-id")
                };
                currentOptions.push(option);
            });

            // remove from  server and re-sequence serial
            if (!await removeOptionFromDb(optionIdToRemove, examId)) return;
            if (!await reSequenceOptionTbl(currentOptions, examId, questionId)) return;

            buildModalOptionsTable(await getOptionsByQuestionId(qoEditModalQuestionId.val()));
        };

        $(document).on("click", ".js-editOptions-modal-remove-option", function (event) {
            bootbox.confirm("Are you sure?", function (result) {
                if (!result) return;

                removeRowOfOptionsTbl($(event.target).closest("tr"));
                addOptionDiv.show();
            });
        });
        /*** END */

        /***  Add option if option count is less than 4 */
        $(document).on("click", "#js-btn-editOptionModal-AddOption", function (event) {
            if (!validation()) { return; }

            bootbox.confirm("Are you sure?", function (result) {
                if (!result) return;

                // save new option to db

                // refresh options table
            });
        });
        /*** END */

        /** Quesiton Option Update, Save change button actions **/
        editQoSubmitBtn.on("click", function () {

        });
        /*END*/
    });
})(jQuery);