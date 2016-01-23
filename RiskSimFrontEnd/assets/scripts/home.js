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
    //// TODO : Get chart data via an API
    var data2 = {};
    data2.SuccessChance = 0.63131784;
    data2.ChartData = {
        labels: ["-1", "+1", "+2", "+3", "+4"],
        datasets: [
            {
                fillColor: "rgba(120, 210, 120, 0.7)",
                strokeColor: "rgba(120, 210, 120, 0.9)",
                highlightFill: "rgba(120, 210, 120, 0.9)",
                highlightStroke: "rgba(120, 210, 120, 1)",
                data: [0.19, 0.21, 0.24, 0.18, 0.1]
            }
        ]
    };
    data2.options = {
        animation: true,
        scaleOverride: true,
        scaleStartValue: 0,
        scaleStepWidth: 0.1,
        scaleSteps: 5
    };

    // Re-enable the simulate button when the sim is done
    $("#simulateButton").attr("disabled", false).html("Simulate");
    $("#results").show();

    // Get context with jQuery - using jQuery's .get() method.
    var ctx = $("#outcomeChart").get(0).getContext("2d");
    // This will get the first returned node in the jQuery collection.
    var outcomeChart = new Chart(ctx).Bar(data.ChartData, data.options);
    for (var i = 0; i < data.ChartData.labels.length; i++) {
        if (data.ChartData.labels[i] < 0)
        {
            outcomeChart.datasets[0].bars[i].fillColor = "rgba(210, 120, 120, 0.7)";
            outcomeChart.datasets[0].bars[i].strokeColor = "rgba(210, 120, 120, 0.9)";
            outcomeChart.datasets[0].bars[i].highlightFill = "rgba(210, 120, 120, 0.9)";
            outcomeChart.datasets[0].bars[i].highlightStroke = "rgba(210, 120, 120, 1)";
        }
        else
        {
            outcomeChart.datasets[0].bars[i].fillColor = "rgba(120, 210, 120, 0.7)";
            outcomeChart.datasets[0].bars[i].strokeColor = "rgba(120, 210, 120, 0.9)";
            outcomeChart.datasets[0].bars[i].highlightFill = "rgba(120, 210, 120, 0.9)";
            outcomeChart.datasets[0].bars[i].highlightStroke = "rgba(120, 210, 120, 1)";
        }
    };

    // Add summary
    $("#resultsSummary").html((data.SuccessChance * 100).toFixed(2) + "% chance of victory");
}