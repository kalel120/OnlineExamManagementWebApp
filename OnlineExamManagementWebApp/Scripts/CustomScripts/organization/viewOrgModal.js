// IIFE
(($) => {
    $(() => {
        const viewOrgModal = $("#js-modal-viewOrg");


        const getTableRowAsObject = (tableRow) => {
            let row = {
                Id: tableRow.find("td:eq(6)").children(".js-viewOrgModalPopup").attr("data-org-id"),
                Name: tableRow.find("td:eq(1)").text(),
                Code: tableRow.find("td:eq(2)").text(),
                Address: tableRow.find("td:eq(3)").text(),
                Contact: tableRow.find("td:eq(4)").text(),
            }
            return row;
        }

        $(document).on("click", ".js-viewOrgModalPopup", (event) => {
            viewOrgModal.modal("toggle");
            const rowData = getTableRowAsObject($(event.target).closest("tr"));
            console.log(rowData);
        });
    });
})(jQuery);