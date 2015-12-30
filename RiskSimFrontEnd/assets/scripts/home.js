function initializeHomeIndex()
{
    // Attach toggles
    $("#attackingOptionsExpand").click(function () {
        $("#optionalAttackingInputs").slideToggle();
        $("#attackingOptionsExpand").toggleClass("glyphicon-plus glyphicon-minus");
    });

    $("#defendingOptionsExpand").click(function () {
        $("#optionalDefendingInputs").slideToggle();
        $("#defendingOptionsExpand").toggleClass("glyphicon-plus glyphicon-minus");
    });

    // Attach simulate button
    $("#simulateButton").click(simulate);
}

// Make call to get simulated result
function simulate()
{
    // Disable the simulate button until this sim is done
    $("#simulateButton").attr("disabled", true).html("Simulating...");

    // TODO

    $("#results").show();
}