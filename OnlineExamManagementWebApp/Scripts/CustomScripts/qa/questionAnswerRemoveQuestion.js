// IIFE to wrap jQuery code

(($) => {
    $(() => {

        $(document).on("click", ".js-removeQuestion", function (event) {
            let eventTarget = $(event.target);

            let closestRow = eventTarget.closest("tr");

            bootbox.confirm({
                size: "large",
                message: "Are you sure? This will delete this particular question",
                callback: async function (confirmed) {
                    if (!confirmed) {
                        return;
                    }

                    // remove from questions table (client)
                    closestRow.remove();

                    // re-sequence on client
                    // marked as removed on server
                    // re-sequence on server
                    // refresh questions table (from serve)
                    if (typeof localStorage !== "undefined") {
                        localStorage.setItem("success","Question Removed");
                    }
                    window.location.reload();
                }
            });
        });
    });
})(jQuery);