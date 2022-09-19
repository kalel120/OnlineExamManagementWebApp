// IIFE

(() => {
    $(() => {
        /**Initialization**/
        const qoEditModal = $("#js-modal-editQo");
        const qoEditModalExamId = $("#js_examId");
        const qoEditModalQuestionId = $("#js-editModal-hidden-qId");
        const qoEditModalForm = $("#js-modal-editQo-form");
        const qoEditModalFormInputs = $("#js-modal-editQo-form :input");
        const qoEditOrderTextBox = $("#js-modal-editQo-order");
        const qoEditMarksTextBox = $("#js-modal-editQo-marks");
        const qoEditDescTextBox = $("#js-modal-editQo-qText");
        const qoEditAddOptionTextBox = $("#js-modal-editQo-oText");
        const qoEditSingleRadioBtn = $("#js-modal-editQo-oType-single");
        const qoEditMultiRadioBtn = $("#js-modal-editQo-oType-multiple");
        const editQoSubmitBtn = $("#js-modal-editQo-Submit");
        const editQoCloseBtn = $(".js-modal-editQo-close");
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

            if (qoEditModalTblBody.find("tr").length === 4) { return; }

            setCheckBoxForSingleAnswer();
            addOptionDiv.show();
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
            qoEditModalTblBody.empty();
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

        const bindNewOptionToTbl = function (data) {
            if (!data) { return; }

            let html = `<tr>`;
            html += `<td>${data.Order}</td>`;
            html += `<td> ${data.Description}</td>`;

            if (data.IsMarkedAsAnswer) {
                html += `<td><input type = "checkbox" value=${data.OptionId} name="OptionEditModalChkBox" checked="checked"/></td>`;
            } else {
                html += `<td><input type = "checkbox" value=${data.OptionId} name="OptionEditModalChkBox"/></td>`;
            }

            html += `<td><a href="#" data-option-id="${data.OptionId}" class="js-editOptions-modal-remove-option btn btn-danger"><i class="avoid-clicks fa fa-trash-o"> Remove </i> </a>`;

            if (!data.OptionId) {
                html += `<a href="#" data-option-id="" class="js-editOptions-modal-tbl-saveOption btn btn-success"><i class="avoid-clicks fa fa-check-square"> Save </i> </a>`;
            }

            html += `</td></tr>`;
            qoEditModalTbl.append(html);
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
        };

        $(document).on("click", ".js-qoEditModalPopup", async function (event) {
            try {
                let tableRow = getRowOfQuestionTableAsObject($(event.target).closest("tr"));
                bindToEditQoModal(tableRow);
                qoEditModalQuestionId.val(tableRow.QuestionId);

                // asynchronously get options data from server
                let options = await getOptionsByQuestionId(tableRow.QuestionId);

                // dynamically build html options table
                options.forEach(bindNewOptionToTbl);

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

        /*** Remove Option from edit modal and db and re-sequence option serial */
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

        const removeTrFromHtml = function(tr) {
            tr.remove();

            qoEditModalTblBody.find("tr").each(function(index, element) {
                $(element).find("td:eq(0)").html(index + 1);
            });
        };

        const removeTrFromDb = async function(optionId, examId) {
            let isRemovedFromDb = await removeOptionFromDb(optionId, examId);
            if (!isRemovedFromDb) { return; }
        };

        const reOrderOptionTbl = async function(examId, questionId) {
            let currentOptions = new Array();

            qoEditModalTblBody.find("tr").each(function (index, element) {
                let option = {
                    Order: index + 1,
                    OptionId: $(element).find("td:eq(3) > a").attr("data-option-id")
                };
                currentOptions.push(option);
            });

            let isReordered = await reSequenceOptionTbl(currentOptions, examId, questionId);
            if (!isReordered) { return; }
        };

        $(document).on("click", ".js-editOptions-modal-remove-option", function (event) {
            bootbox.confirm("Are you sure?", function (result) {
                if (!result) return;

                let tr = $(event.target).closest("tr");
                let optionId = tr.find("td:eq(3) > a").data("option-id");

                if (!optionId) {
                    removeTrFromHtml(tr);
                } else {
                    removeTrFromHtml(tr);
                    removeTrFromDb(optionId, qoEditModalExamId.val());
                    reOrderOptionTbl(qoEditModalExamId.val(), qoEditModalQuestionId.val());
                }
                //removeRowOfOptionsTbl();
                addOptionDiv.show();
            });
        });
        /*** END */

        /***  Add option if option count is less than 4 */
        const getSingleOptionToBindOnHtmlTbl = function () {
            return {
                Order: qoEditModalTblBody.find("tr").length + 1,
                Description: $.trim(qoEditAddOptionTextBox.val()),
                QuestionId: $.trim(qoEditModalQuestionId.val()),
                ExamId: $.trim(qoEditModalExamId.val()),
                OptionId: null
                //,IsMarkedAsAnswer: $("#js-modal-editQo-AddOption-isCorrect").prop("checked") ? true : false
            }
        };

        const getSingleOptionToSaveOnDb = function (tr) {
            return {
                SerialNo: tr.find("td:eq(0)").text(),
                OptionText: tr.find("td:eq(1)").text(),
                IsCorrectAnswer: tr.find("td:eq(2)").find("input[type='checkbox']").prop("checked"),
                ExamId: $.trim(qoEditModalExamId.val()),
                QuestionId: $.trim(qoEditModalQuestionId.val())
            }
        };

        const saveSingleOptionToDb = function (option) {
            return $.ajax({
                type: "POST",
                url: `/QuestionAnswer/SaveSingleOption`,
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(option)
            })
                .fail(function (res, textStatus, errorThrown) {
                    console.log(res);
                    console.log(textStatus);
                    console.log(errorThrown);
                });
        };

        $(document).on("click", "#js-btn-editOptionModal-AddOption", function (event) {
            if (!validation()) { return; }

            let option = getSingleOptionToBindOnHtmlTbl();

            bindNewOptionToTbl(option);

            // hide add option button based on table row count
            if (qoEditModalTblBody.find("tr").length === 4 && addOptionDiv.is(":visible")) {
                addOptionDiv.hide();
            }
        });
        /*** END */

        /** Save newly added option to db **/
        $(document).on("click", ".js-editOptions-modal-tbl-saveOption", async function (event) {
            // build savable object
            let tr = $(event.target).closest("tr");
            let singleOptionToSaveDb = getSingleOptionToSaveOnDb(tr);

            // save single option to db
            if (!await saveSingleOptionToDb(singleOptionToSaveDb)) { return; }

            // remove individual save option button from table column
            tr.find("a.js-editOptions-modal-tbl-saveOption").remove();
        });

        /***END*/


        /** Quesiton Option Update, Save change button actions **/
        const anyUnsavedOption = function () {
            let isUnsaved = false;

            qoEditModalTblBody.find("tr").each(function (index, element) {
                if ($(element).find("td:eq(3) > a").hasClass("js-editOptions-modal-tbl-saveOption")) {
                    isUnsaved = true;
                    return;
                }
            });
            return isUnsaved;
        };

        editQoSubmitBtn.on("click", function () {
            if (anyUnsavedOption() || qoEditModalTblBody.find("tr").length !== 4) {
                bootbox.alert("Still unsaved option left or options are less than 4");
                return;
            }

        });
        /*END*/

        /** Modal close button actions*/
        editQoCloseBtn.on("click", function () {
            if (anyUnsavedOption()) {
                bootbox.alert("Still unsaved option left");
                editQoCloseBtn.removeAttr("data-dismiss");
                return;
            }
            editQoCloseBtn.attr("data-dismiss", "modal");
        });
        /**END*/
    });
})(jQuery);