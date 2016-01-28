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

    // Note that charts should be responsive
    Chart.defaults.global.responsive = true;
}

// Make call to get simulated result
function simulate()
{
    // Disable the simulate button until this sim is done
    $("#simulateButton").attr("disabled", true).html("Simulating...");

    $.ajax({
        url: "Data/SimulateAttack",
        data: $("form").serialize(),
        processData: false, // Don't process the data into the query string; use the request body
        type: "POST",
        success: PopulateResults,
        error: function (jqXHR, textStatus, errorThrown) {
            // Re-enable the simulate button when the sim is done
            $("#simulateButton").attr("disabled", false).html("Simulate");
            $("#results").show();

            alert("ERROR: " + textStatus + " " + errorThrown);
        }
    });
}

function PopulateResults(data)
{
    // Re-enable the simulate button when the sim is done
    $("#simulateButton").attr("disabled", false).html("Simulate");
    $("#results").show();

    // Get context with jQuery - using jQuery's .get() method.
    var ctx = $("#outcomeChart").get(0).getContext("2d");

    if (window.chart) window.chart.destroy();
    window.chart = new Chart(ctx).Bar(data.ChartData, data.options);

    for (var i = 0; i < data.ChartData.labels.length; i++) {
        if (data.ChartData.labels[i] < 0)
        {
            window.chart.datasets[0].bars[i].fillColor = "rgba(210, 120, 120, 0.7)";
            window.chart.datasets[0].bars[i].strokeColor = "rgba(210, 120, 120, 0.9)";
            window.chart.datasets[0].bars[i].highlightFill = "rgba(210, 120, 120, 0.9)";
            window.chart.datasets[0].bars[i].highlightStroke = "rgba(210, 120, 120, 1)";
        }
        else
        {
            window.chart.datasets[0].bars[i].fillColor = "rgba(120, 210, 120, 0.7)";
            window.chart.datasets[0].bars[i].strokeColor = "rgba(120, 210, 120, 0.9)";
            window.chart.datasets[0].bars[i].highlightFill = "rgba(120, 210, 120, 0.9)";
            window.chart.datasets[0].bars[i].highlightStroke = "rgba(120, 210, 120, 1)";
        }
    };
    window.chart.update();

    // Add summary
    $("#resultsSummary").html((data.SuccessChance * 100).toFixed(2) + "% chance of victory");
}