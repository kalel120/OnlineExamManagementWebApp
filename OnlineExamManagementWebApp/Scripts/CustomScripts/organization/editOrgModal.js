// IIFE

(($) => {
    $(() => {
        const editOrgModal = $("#js-modal-editOrg");

        const getTableRowAsObject = (tableRow) => {
            return {
                Id: tableRow.find("td:eq(7)").children(".js-viewOrgModalPopup").attr("data-org-id"),
                Name: tableRow.find("td:eq(1)").text(),
                Code: tableRow.find("td:eq(2)").text(),
                Address: tableRow.find("td:eq(3)").text(),
                Contact: tableRow.find("td:eq(4)").text()
            };
        }

        const bindToEditOrgModal = (data) => {
            $("#js-modal-editOrg-name").val(data.Name);
            $("#js-modal-editOrg-code").val(data.Code);
            $("#js-modal-editOrg-address").val(data.Address);
            $("#js-modal-editOrg-contact").val(data.Contact);
        }

        $(document).on("click", ".js-editOrgModalPopup", (event) => {
            setTimeout(() => {
                let tableRow = getTableRowAsObject($(event.target).closest("tr"));
                bindToEditOrgModal(tableRow);
                editOrgModal.modal("toggle");
            },300);
        });
    });
})(jQuery);