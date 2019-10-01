// IIFE

(($) => {
    $(() => {
        const editOrgModal = $("#js-modal-editOrg");
        const btnEditOrgSubmit = $("#js-modal-editOrg-Submit");
        const editOrgModalForm = $("#js-modal-editOrg-form");


        //validation code
        const validation = () => {
            return editOrgModalForm.validate({
                errorClass: "text-danger",
                errorElement: "div",

                rules: {
                    "Name": {
                        required: true
                        , minlength: 3
                        , maxlength: 50
                    },
                    "Code": {
                        required: true
                        , minlength: 3
                        , maxlength: 20
                    },
                    "Address": {
                        required: true
                        , minlength: 3
                        , maxlength: 100
                    },
                    "Contact": {
                        required: true
                        , minlength: 3
                        , maxlength: 100
                    }
                },
                messages: {
                    "Name": {
                        required: "Name Required"
                        , minlength: "Should not be less than {0} character"
                        , maxlength: "Should not be more than {0} character"
                    },
                    "Code": {
                        required: "Code Required"
                    },
                    "Address": {
                        required: "Address Required"
                        , minlength: "Should not be less than  {0} character"
                        , maxlength: "Should not be more than {0} character"

                    },
                    "Contact": {
                        required: "Contact No. Required"
                        , minlength: "Should not be less than  {0} character"
                        , maxlength: "Should not be more than {0} character"
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
        }
        // validation end

        const getTableRowAsObject = (tableRow) => {
            let content = {
                Id: tableRow.find("input.btn.btn-warning.js-editOrgModalPopup").attr("data-org-id"),
                Name: $.trim(tableRow.find("td:eq(1)").text()),
                Code: $.trim(tableRow.find("td:eq(2)").text()),
                Address: $.trim(tableRow.find("td:eq(3)").text()),
                Contact: $.trim(tableRow.find("td:eq(4)").text())
            };
            
            return content;
        }

        const bindToEditOrgModal = (data) => {
            $("#js-modal-editOrg-name").val(data.Name);
            $("#js-modal-editOrg-code").val(data.Code);
            $("#js-modal-editOrg-address").val(data.Address);
            $("#js-modal-editOrg-contact").val(data.Contact);
            btnEditOrgSubmit.data("orgId", data.Id);
        }

        $(document).on("click", ".js-editOrgModalPopup", (event) => {
            setTimeout(() => {
                let tableRow = getTableRowAsObject($(event.target).closest("tr"));
                console.log($(event.target).data("org-id"));
                bindToEditOrgModal(tableRow);
                editOrgModal.modal("toggle");
            }, 300);
        });

        const updateOrganization = async (content, orgId) => {
            let result = await $.post("/Organization/UpdateOrganization/", { dto: content, orgId: orgId });
            if (result) {
                alert("Updated Successfully!");
                location.reload(true);
            } else {
                alert("Update Failed");
            }
            
        }

        const getFormData = (form) => {
            let unIndexedArray = form.serializeArray();
            let indexedArray = {};

            $.map(unIndexedArray, function (n, i) {
                indexedArray[n["name"]] = n["value"];
            });

            return indexedArray;
        }

        btnEditOrgSubmit.on("click", () => {
            if (validation()) {
                updateOrganization(getFormData(editOrgModalForm), btnEditOrgSubmit.data("orgId"));
            }
        });
    });
})(jQuery);