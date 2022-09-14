// IIFE

(() => {
    $(() => {
        /**Initialization**/
        const qoEditModal = $("#js-modal-editQo");
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
        const addOptionDiv = $(".js-div-addOption-editQoModal");

        const resetModalState = function() {
            editQoSubmitBtn.hide();
            addOptionDiv.hide();
        }

        resetModalState();
        /*END*/

        /*** Modal Reset*/
        qoEditModal.on("hidden.bs.modal", function() {
            $(this).find("form").trigger("reset");
            resetModalState();
        });

        /**END */

        /** On any input change, will trigger an event to enable disable save button */
        qoEditModalFormInputs.on("keyup change ifChanged", function () {
            editQoSubmitBtn.show();
        });

        qoEditModalTbl.on("change", function() {
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

            // dynamically generated tablebody
            $("#js-tbl-editOptionModal-tbody").empty();
            for (let index = 0; index < data.Options.length; index++) {
                let html = `<tr>
                            <td>${data.Options[index].Order}</td>
                            <td> ${data.Options[index].Description}</td>`;

                if (data.Options[index].IsMarkedAsAnswer) {
                    html += `<td><input type = "checkbox" name="OptionEditModalChkBox" checked="${data.Options[index].IsMarkedAsAnswer}"/></td>`;
                } else {
                    html += `<td><input type = "checkbox" name="OptionEditModalChkBox"/></td>`;
                }
                html += `<td><a href="#" class="js-editOptions-modal-remove-option btn btn-danger"><i class="avoid-clicks fa fa-trash-o"> Remove </i> </a> </td>
                         </tr>`;


                qoEditModalTbl.append(html);
            }
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

        /**
         * Remove Option from edit modal and db
         */
        $(document).on("click", ".js-editOptions-modal-remove-option", function(event) {
            bootbox.confirm("Are you sure?", function(result) {
                if (result) {
                    addOptionDiv.show();
                }
            });
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