﻿// IIFE

(() => {
    $(() => {
        /**Initialization**/
        const $EXAM_ID = $.trim($("#js_examId").val());
        const $QUESTION_ID = $("#js-editModal-hidden-qId");

        const qoEditModal = $("#js-modal-editQo");
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
        qoEditModal.on("shown.bs.modal", function () {
            if (qoEditModalTblBody.find("tr").length === 4) {
                return;
            }
            addOptionDiv.show();
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
        qoEditModalFormInputs.on("keyup change", function () {
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

            if (qoEditSingleRadioBtn.prop("checked")) {
                if (data.IsMarkedAsAnswer) {
                    html += `<td><input type="radio" value=${data.OptionId} name="OptionEditModalAnsSelect" checked/> </td>`;
                } else {
                    html += `<td><input type="radio" value=${data.OptionId} name="OptionEditModalAnsSelect"/> </td>`;
                }

            }

            if (qoEditMultiRadioBtn.prop("checked")) {
                if (data.IsMarkedAsAnswer) {
                    html += `<td><input type = "checkbox" value=${data.OptionId} name="OptionEditModalAnsSelect" checked="checked"/></td>`;
                } else {
                    html += `<td><input type = "checkbox" value=${data.OptionId} name="OptionEditModalAnsSelect"/></td>`;
                }
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
                qoEditSingleRadioBtn.prop("checked", true);
                qoEditMultiRadioBtn.prop("checked", false);
            }

            if (data.OptionType === qoEditMultiRadioBtn.val()) {
                qoEditMultiRadioBtn.prop("checked", true);
                qoEditSingleRadioBtn.prop("checked", false);
            }
        };

        $(document).on("click", ".js-qoEditModalPopup", async function (event) {
            try {
                let tableRow = getRowOfQuestionTableAsObject($(event.target).closest("tr"));
                bindToEditQoModal(tableRow);
                $QUESTION_ID.val(tableRow.QuestionId);

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

        const removeTrFromHtml = function (tr) {
            tr.remove();

            qoEditModalTblBody.find("tr").each(function (index, element) {
                $(element).find("td:eq(0)").html(index + 1);
            });
        };

        const reOrderOptionTbl = async function (examId, questionId) {
            let tableBodyRowCount = qoEditModalTblBody.children().length;
            if (tableBodyRowCount === 0) {
                bootbox.alert("No options are assigned to this question");
                return;
            }

            let currentOptions = new Array();
            qoEditModalTblBody.find("tr").each(function (index, element) {
                let optionId = $(element).find("td:eq(3) > a").data("option-id");
                if (!optionId) { return; }

                let option = {
                    Order: index + 1,
                    OptionId: $(element).find("td:eq(3) > a").attr("data-option-id")
                };
                currentOptions.push(option);
            });

            let isReordered = await reSequenceOptionTbl(currentOptions, examId, questionId);
            if (!isReordered) {
                bootbox.alert("Re-Order Options failed!");
                return;
            }
        };

        $(document).on("click", ".js-editOptions-modal-remove-option", function (event) {

            bootbox.confirm("Are you sure?", async function (result) {
                if (!result) return;

                let tr = $(event.target).closest("tr");
                let optionId = tr.find("td:eq(3) > a").data("option-id");

                if (!optionId) {
                    removeTrFromHtml(tr);
                }
                else {
                    removeTrFromHtml(tr);

                    let isRemovedFromDb = await removeOptionFromDb(optionId, $EXAM_ID);
                    if (!isRemovedFromDb) { return; }

                    reOrderOptionTbl($EXAM_ID, $QUESTION_ID);
                }
                addOptionDiv.show();
            });
        });
        /*** END */

        /***  Add option if option count is less than 4 */
        const getSingleOptionToBindOnHtmlTbl = function () {
            return {
                Order: qoEditModalTblBody.find("tr").length + 1,
                Description: $.trim(qoEditAddOptionTextBox.val()),
                QuestionId: $QUESTION_ID.val(),
                ExamId: $EXAM_ID,
                OptionId: null
            }
        };

        const getSingleOptionToSaveOnDb = function (tr) {
            return {
                SerialNo: tr.find("td:eq(0)").text(),
                OptionText: tr.find("td:eq(1)").text(),
                IsCorrectAnswer: tr.find("td:eq(2)").find("input[type='checkbox']").prop("checked"),
                ExamId: $EXAM_ID,
                QuestionId: $QUESTION_ID.val()
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

        const isOptionTextExistsOnTable = function (newOptionText) {
            let result = false;

            qoEditModalTblBody.find("tr").each(function (index, element) {
                let tblOptionText = $.trim($(element).find("td:eq(1)").text().toLowerCase());
                if (newOptionText === tblOptionText) {
                    result = true;
                    return;
                }
            });
            return result;
        };

        $(document).on("click", "#js-btn-editOptionModal-AddOption", function (event) {
            if (!validation()) { return; }

            let option = getSingleOptionToBindOnHtmlTbl();

            if (isOptionTextExistsOnTable(option.Description.toLowerCase())) {
                bootbox.alert("Option Description already exists on this table");
                return;
            }

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

        /** Change option type selection **/
        // Update Answer selection type of Question by exam and question id
        const updateAnswerSelectionType = async function (optionType) {
            let reqData = {
                OptionType: optionType,
                ExamId: $EXAM_ID,
                QuestionId: $QUESTION_ID.val()
            };
            
            try {
                let response = await $.ajax({
                    type: "PUT",
                    url: "/QuestionAnswer/UpdateOptionTypeOfQuestion",
                    dataType: "json",
                    data: reqData
                });

                return true;
            } catch (error) {
                if (error.status === 500) {
                    window.location = "/Error/Error500";
                }
                return false;
            }
        };

        $(document).on("change", "input[name = 'OptionTypeEditModal']", function (event) {
            bootbox.confirm({
                size: "small",
                message: "Are You Sure? This will clear out already selected correct answers",
                callback: function (result) {
                    if (!result) {
                        $("input[name = 'OptionTypeEditModal']").not(':checked').prop("checked", true);
                        return;
                    }

                    qoEditModalTblBody.find(".js-editOptions-modal-tbl-updateAnswer").remove();

                    if (qoEditModalTblBody.find("input[type='radio']").length > 0) {
                        qoEditModalTblBody.find("input[type='radio']").attr("type", "checkbox");

                        //uncheck all checkboxes
                        qoEditModalTblBody.find("input[type='checkbox']").prop("checked", false);

                        //update option type on server
                        updateAnswerSelectionType(qoEditMultiRadioBtn.val());
                    } else {
                        qoEditModalTblBody.find("input[type='checkbox']").attr("type", "radio");

                        //uncheck radio button
                        qoEditModalTblBody.find("input[type='radio']").prop("checked", false);

                        //update option type on server
                        updateAnswerSelectionType(qoEditSingleRadioBtn.val());
                    }
                }
            });

        });
        /**END**/

        /** Show unsaved state if correct answer selection is modified **/
        const isRemovedUpdateCorrectAnsBtn = function (optionType, thisUpdateAnsClass) {
            let result = false;
            const tbodyUpdateAnsClass = qoEditModalTblBody.find(".js-editOptions-modal-tbl-updateAnswer");

            // for single answer option type
            if (optionType === "single") {
                if (tbodyUpdateAnsClass.length !== 0) {
                    tbodyUpdateAnsClass.remove();
                    result = true;
                }
            }

            // for multiple answer option type
            if (optionType === "multiple") {
                if ((thisUpdateAnsClass.length >= 1)) {
                    thisUpdateAnsClass.remove();
                    result = true;
                }
            }

            return result;
        };

        const appendUpdateCorrectAnsBtn = function (optionId, eventTarget) {
            const html = `<a href="#" data-option-id="${optionId}" class="js-editOptions-modal-tbl-updateAnswer btn btn-primary"><i class="avoid-clicks fa fa-database"> Update Answer </i> </a>`;

            eventTarget.closest("tr").find("td:eq(3)").append(html);
        };

        const handleUpdateAnsBtnForSingleOptionType = function (checkedAttr, thisUpdateAnsClass, optionId, eventTarget) {
            isRemovedUpdateCorrectAnsBtn("single", thisUpdateAnsClass);

            // append update correct answer if no 'checked' attr exists and no update-answer button exists
            if ((typeof checkedAttr === "undefined") && thisUpdateAnsClass.length === 0) {
                appendUpdateCorrectAnsBtn(optionId, eventTarget);
            }
        };

        const handleUpdateAnsBtnForMultiOptionType = function (checkedAttr, thisUpdateAnsClass, optionId, eventTarget) {
            if (!isRemovedUpdateCorrectAnsBtn("multiple", thisUpdateAnsClass)) {

                //  if this answer is saved on db and user has checked this as correct answer then return. Otherwise display update-ans button
                if ((typeof checkedAttr !== "undefined") && eventTarget.prop("checked")) {
                    return;
                }
                appendUpdateCorrectAnsBtn(optionId, eventTarget);
            }
        };

        $(document).on("change", "input[name='OptionEditModalAnsSelect']", function (event) {
            let eventTarget = $(event.target);
            let thisUpdateAnsClass = eventTarget.closest("tr").find(".js-editOptions-modal-tbl-updateAnswer");

            if (qoEditSingleRadioBtn.prop("checked")) {
                handleUpdateAnsBtnForSingleOptionType(eventTarget.attr("checked"), thisUpdateAnsClass, eventTarget.val(), eventTarget);
                return;
            }
            if (qoEditMultiRadioBtn.prop("checked")) {
                handleUpdateAnsBtnForMultiOptionType(eventTarget.attr("checked"), thisUpdateAnsClass, eventTarget.val(), eventTarget);
                return;
            }
        });
        /** END **/

        /** Save updated correct answer **/
        const updateCorrectAnsById = async function (dataToUpdate) {

            try {
                let response = await $.ajax({
                    type: "PUT",
                    url: "/QuestionAnswer/UpdateCorrectAnsOfOption",
                    dataType: "json",
                    data: dataToUpdate
                });

                return true;

            } catch (error) {
                // Enable this before going live
                //if (error.status === 500) {
                //    window.location = "/Error/Error500";
                //}
                return false;
            }
        };

        const isCorrectAnswerSaved = function (eventTarget) {
            let optionId = eventTarget.find("td:eq(2)").children("input[name='OptionEditModalAnsSelect']").val();

            // build object to send to server
            let optionToUpdate = {
                OptionId: optionId
                , QuestionId: $QUESTION_ID.val()
                , ExamId: $EXAM_ID
                , OptionType: "Single Answer"
            };

            return updateCorrectAnsById(optionToUpdate);
        };

        $(document).on("click", ".js-editOptions-modal-tbl-updateAnswer", function (event) {
            // for single answer type
            if (qoEditSingleRadioBtn.prop("checked")) {
                if (!isCorrectAnswerSaved($(event.target).closest("tr"))) {
                    return;
                }
                $(event.target).remove();
                return;
            }

            // for multiple answer type
            bootbox.alert("Multiple answer is not yet implemented");
            return;
        });
        /** END **/

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

        const anyUncheckedCorrectAns = function () {
            let isUnchecked = false;

            let notCheckedLength = $("input[name='OptionEditModalAnsSelect']").not(":checked").length;
            if (notCheckedLength !== 0) {
                isUnchecked = true;
            }
            return isUnchecked;
        };

        editQoSubmitBtn.on("click", function () {
            if (anyUncheckedCorrectAns()) {
                bootbox.alert("Still unanswered option left");
                return;
            }

            if (anyUnsavedOption()) {
                bootbox.alert("Still unsaved options left");
                return;
            }

            if (qoEditModalTblBody.find("tr").length !== 4) {
                bootbox.alert("Options are less than 4");
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