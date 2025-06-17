$(document).ready(function () {
    var currentGfgStep, nextGfgStep, previousGfgStep;
    var opacity;
    var current = 1;
    var steps = $("fieldset").length;

    setProgressBar(current);

    $(".next-step").click(function () {
        //currentGfgStep = $(this).parent();
        //nextGfgStep = $(this).parent().next();
        currentGfgStep = $(this).closest('fieldset');
        nextGfgStep = $(this).closest('fieldset').next();

        var stepName = currentGfgStep.data('name');

        var isValid = true;

        if (stepName === "wizard_gen") {
            var nameField = currentGfgStep.find("input[name='Name']");
            if (!nameField.val()) {
                nameField.addClass("input-error");
                isValid = false;
            } else {
                nameField.removeClass("input-error");
            }
        }

        if (stepName === "wizard_passcode") {
            var passcodesettings = currentGfgStep.find("input[name='ckbCheckTogglePasscodeSettings']");
            if (!passcodesettings.is(":checked"))
            {
                var MinPassField = currentGfgStep.find("input[name='MinPass']");
                if (!MinPassField.val()) {
                    MinPassField.addClass("input-error");
                    isValid = false;
                } else {
                    MinPassField.removeClass("input-error");
                }

                var MinCCField = currentGfgStep.find("input[name='MinCC']");
                if (!MinCCField.val()) {
                    MinCCField.addClass("input-error");
                    isValid = false;
                } else {
                    MinCCField.removeClass("input-error");
                }

                var MaxPassAgeField = currentGfgStep.find("input[name='MaxPassAge']");
                if (!MaxPassAgeField.val()) {
                    MaxPassAgeField.addClass("input-error");
                    isValid = false;
                } else {
                    MaxPassAgeField.removeClass("input-error");
                }


                var MaximumAutoLockField = currentGfgStep.find("input[name='MaximumAutoLock']");
                if (!MaximumAutoLockField.val()) {
                    MaximumAutoLockField.addClass("input-error");
                    isValid = false;
                } else {
                    MaximumAutoLockField.removeClass("input-error");
                }

                var PassHistoryField = currentGfgStep.find("input[name='PassHistory']");
                if (!PassHistoryField.val()) {
                    PassHistoryField.addClass("input-error");
                    isValid = false;
                } else {
                    PassHistoryField.removeClass("input-error");
                }

                var MaxGPeriodField = currentGfgStep.find("input[name='MaxGPeriod']");
                if (!MaxGPeriodField.val()) {
                    MaxGPeriodField.addClass("input-error");
                    isValid = false;
                } else {
                    MaxGPeriodField.removeClass("input-error");
                }

                var MaxAttemField = currentGfgStep.find("input[name='MaxAttem']");
                if (!MaxAttemField.val()) {
                    MaxAttemField.addClass("input-error");
                    isValid = false;
                } else {
                    MaxAttemField.removeClass("input-error");
                }
            }
        }
        
        if (stepName === "wizard_cell") {
            var cellsettings = currentGfgStep.find("input[name='ckbNotRequiredCell']");
            if (!cellsettings.is(":checked")) {
                var APNTypeField = currentGfgStep.find("input[name='APNType']");
                if (APNTypeField.val() === "default") {
                    var DeAPNField = currentGfgStep.find("input[name='DeAPN']");
                    if (!DeAPNField.val()) {
                        DeAPNField.addClass("input-error");
                        isValid = false;
                    } else {
                        DeAPNField.removeClass("input-error");
                    }
                }
                else if (APNTypeField.val() === "Data") {
                    var DataANField = currentGfgStep.find("input[name='DataAN']");
                    if (!DataANField.val()) {
                        DataANField.addClass("input-error");
                        isValid = false;
                    } else {
                        DataANField.removeClass("input-error");
                    }
                }
                else {
                    var DeAPNField = currentGfgStep.find("input[name='DeAPN']");
                    if (!DeAPNField.val()) {
                        DeAPNField.addClass("input-error");
                        isValid = false;
                    } else {
                        DeAPNField.removeClass("input-error");
                    }
                    var DataANField = currentGfgStep.find("input[name='DataAN']");
                    if (!DataANField.val()) {
                        DataANField.addClass("input-error");
                        isValid = false;
                    } else {
                        DataANField.removeClass("input-error");
                    }
                }


            }
        }


        if (!isValid) {
            return; // Stop if validation fails
        }

        $("#progressbar li").eq($("fieldset")
            .index(nextGfgStep)).addClass("active");

        nextGfgStep.show();
        currentGfgStep.animate({
            opacity: 0
        }, {
            step: function (now) {
                opacity = 1 - now;

                currentGfgStep.css({
                    'display': 'none',
                    'position': 'relative'
                });
                nextGfgStep.css({
                    'opacity': opacity
                });
            },
            duration: 500
        });
        setProgressBar(++current);
    });

    $(".previous-step").click(function () {

        //currentGfgStep = $(this).parent();
        //previousGfgStep = $(this).parent().prev();
        currentGfgStep = $(this).closest('fieldset');
        previousGfgStep = $(this).closest('fieldset').prev();

        $("#progressbar li").eq($("fieldset")
            .index(currentGfgStep)).removeClass("active");

        previousGfgStep.show();

        currentGfgStep.animate({
            opacity: 0
        }, {
            step: function (now) {
                opacity = 1 - now;

                currentGfgStep.css({
                    'display': 'none',
                    'position': 'relative'
                });
                previousGfgStep.css({
                    'opacity': opacity
                });
            },
            duration: 500
        });
        setProgressBar(--current);
    });

    /*$("li[id^='step']").click(function (event) {
        let stepId = event.target.id;
        var stepNum = Number(stepId.split("step")[1]);

        var targetFieldset = $("#wizard_container fieldset:nth-child(" + (stepNum + 1) + ")");

        //current = stepNum;
        //setProgressBar(current);
    })*/

    function setProgressBar(currentStep) {
        var percent = parseFloat(100 / steps) * current;
        percent = percent.toFixed();
        $(".progress-bar")
            .css("width", percent + "%")
    }

    $(".submit").click(function () {
        return false;
    })

    $("input.numeric-only").on("input", function () {
        this.value = this.value.replace(/[^0-9]/g, '');
    });

    $(document).on('blur', 'input.input-error', function () {
        if ($(this).val().trim() !== '') {
            $(this).removeClass('input-error');
        }
    });
});
