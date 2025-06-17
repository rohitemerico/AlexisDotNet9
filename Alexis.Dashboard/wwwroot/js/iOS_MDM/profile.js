$(document).ready(function () {
    TogglePasscodeSettings();
    ToggleFunctionSettings();
    if ($('#RestrictAppsUsage').val() === 'AllowAllApps') {
        $('#pnlRestrictionApp').hide();
    }
    else {
        $('#pnlRestrictionApp').show();
    }
    if ($('#ckbNotRequiredWifi').is(':checked')) {
        $('#btnAddWifi').addClass('no-click');
        $('#btnAddWifi').attr("disabled", "disabled");

    } else {
        $('#btnAddWifi').attr("disabled", false);
        $('#btnAddWifi').removeClass('no-click');
    }
    if ($($('#ckbNotRequiredVPN')).is(':checked')) {
        $('#btnAddVPN').addClass('no-click');
        $('#btnAddVPN').attr("disabled", "disabled");

    } else {
        $('#btnAddVPN').attr("disabled", false);
        $('#btnAddVPN').removeClass('no-click');
    }
    //Cellular
    ToggleCellularSettings();
    if ($('#APNType').val() === 'Data') {
        $('#pnlData').show();
        $('#defaultAPN').hide();
    }
    else if ($('#APNType').val() === 'DD') {
        $('#DD_divider').show();
        $('#pnlData').show();
        $('#defaultAPN').show();
    }
    else {
        $('#pnlData').hide();
        $('#defaultAPN').show();
    }
});



function ddlConnType_SelectedIndexChanged() {
    if ($('#ConnectionType').val() === 'L2TP') {
        $('#pnlL2TP').show();
        $('#pnlIpSec').hide();
    }
    else {
        $('#pnlL2TP').hide();
        $('#pnlIpSec').show();
    }
    ValidateVPN();
}

function txtGroupNameL2TP_TextChanged() {
    if ($('#L2TP_GroupName').val() != "") {
        $('#pnlHybridL2TP').show();
    }
    else {
        $('#pnlHybridL2TP').hide();
    }
}

function txtGroupNameIpSec_TextChanged() {
    if ($('#IPSEC_GroupName').val() != "") {
        $('#pnlHybrid').show();
    }
    else {
        $('#pnlHybrid').hide();
    }
    ValidateVPN();
}

function ddlProxySetupVPNL2TP_Changed() {
    if ($('#L2TP_ProxySetup').val() === 'None') {
        $('#pnlManualVPN').hide();
        $('#pnlAutomatic').hide();
    }
    else if ($('#L2TP_ProxySetup').val() === 'Manual') {
        $('#pnlManualVPN').show();
        $('#pnlAutomatic').hide();
    }
    else {
        $('#pnlManualVPN').hide();
        $('#pnlAutomatic').show();
    }
    ValidateVPN();
}


function ddlProxySetupVPNIPSEC_Changed() {
    if ($('#IPSEC_ProxySetup').val() === 'None') {
        $('#pnlManualIPSECVPN').hide();
        $('#pnlAutomaticIPSEC').hide();
    }
    else if ($('#IPSEC_ProxySetup').val() === 'Manual') {
        $('#pnlManualIPSECVPN').show();
        $('#pnlAutomaticIPSEC').hide();
    }
    else {
        $('#pnlManualIPSECVPN').hide();
        $('#pnlAutomaticIPSEC').show();
    }
    ValidateVPN();
}


function onChange_ddlProxySetup() {
    ValidateWiFi();
    if ($('#ProxySetup').val() === 'Manual') {
        $('#pnlManualPS').show();
        $('#pnlAutomaticWiFi').hide();
    }
    else if ($('#ProxySetup').val() === 'Automatic') {
        $('#pnlManualPS').hide();
        $('#pnlAutomaticWiFi').show();
    }
    else {
        $('#pnlManualPS').hide();
        $('#pnlAutomaticWiFi').hide();
    }
}

function onChange_ddlSecurityType() {
    ValidateWiFi();
    $('#pnlPasswordWiFi').show();
    if ($('#SecurityType').val() === 'None') {
        $('#pnlPasswordWiFi').hide();
    }
}

function onChange_ddlFastLaneWifi() {
    $('#pnlResQos').hide();
    if ($('#FastLaneQosmarking').val() === 'Restrict') {
        $('#pnlResQos').show();
    }
}

$(document).on('change', '#ckbSelectall', function () {
    $('#branchList input[type="checkbox"]').prop('checked', this.checked);
});

$(document).on('change', '#branchList input[type="checkbox"]', function () {
    // If any checkbox is unchecked, uncheck the "Select All"
    if (!this.checked) {
        $('#ckbSelectall').prop('checked', false);
    } else {
        // If all checkboxes are checked, check the "Select All"
        const allChecked = $('#branchList input[type="checkbox"]').length ===
            $('#branchList input[type="checkbox"]:checked').length;
        $('#ckbSelectall').prop('checked', allChecked);
    }
});

function BtnBranch_Click() {
    $('#myModal_BranchID').modal('show');
}



function PopupRedirect(value) {
    alert(value);
    window.location = "iOS_MDMCheckerMaker";
};

function ShowLoading() {
    document.getElementById('wizard_container').style.opacity = 0.2;
    var loader = document.getElementById('loader_container');
    loader.style.opacity = 1;
    loader.style.display = "";
}

function PreventDouble() {

    ShowLoading(); //show spinner

    if (document.getElementById('<%=btnSubmit.ClientID%>').disabled != true) {
        document.getElementById('<%=btnSubmit.ClientID%>').disabled = true;
        return true;
    }
    else {

        return false;
    }
};

$('#ckbNotRequiredWifi').change(function () {
    if ($(this).is(':checked')) {
        $('#btnAddWifi').addClass('no-click');
        $('#btnAddWifi').attr("disabled", "disabled");

    } else {
        $('#btnAddWifi').attr("disabled", false);
        $('#btnAddWifi').removeClass('no-click');
    }
});

$('#ckbNotRequiredVPN').change(function () {
    if ($(this).is(':checked')) {
        $('#btnAddVPN').addClass('no-click');
        $('#btnAddVPN').attr("disabled", "disabled");

    } else {
        $('#btnAddVPN').attr("disabled", false);
        $('#btnAddVPN').removeClass('no-click');
    }
});


$('#APNType').change(function () {
    var selectedValue = $(this).val();
    if (selectedValue === 'Data') {
        $('#pnlData').show();
        $('#defaultAPN').hide();
    }
    else if (selectedValue === 'DD') {
        $('#DD_divider').show();
        $('#pnlData').show();
        $('#defaultAPN').show();
    }
    else {
        $('#pnlData').hide();
        $('#defaultAPN').show();
    }
});

$('#RestrictAppsUsage').change(function () {
    var selectedValue = $(this).val();
    if (selectedValue === 'AllowAllApps') {
        $('#pnlRestrictionApp').hide();
    }
    else {
        $('#pnlRestrictionApp').show();
    }
});

function TogglePasscodeSettings() {
    if ($("#ckbCheckTogglePasscodeSettings").is(':checked')) {
        $('#wizard_passcode').find("input,textarea,select,button").attr("disabled", "disabled");
        document.getElementById('ckbCheckTogglePasscodeSettings').disabled = false;
        $("fieldset[data-name='wizard_passcode']").find(".input-error").removeClass("input-error");
    }
    else {
        $('#wizard_passcode').find("input,textarea,select").attr("disabled", false);
    }
}

function ToggleFunctionSettings() {
    if ($("#ckbCheckAllFunctionality").is(':checked')) {
        $('#wizard_func').find("input,textarea,select,button").attr("disabled", "disabled");
        document.getElementById('ckbCheckAllFunctionality').disabled = false;
    }
    else {
        $('#wizard_func').find("input,textarea,select").attr("disabled", false);
    }
};


function ToggleCellularSettings() {
    if ($("#ckbNotRequiredCell").is(':checked')) {
        $('#wizard_cell').find("input,textarea,select,button").attr("disabled", "disabled");
        document.getElementById('ckbNotRequiredCell').disabled = false;
        $("fieldset[data-name='wizard_cell']").find(".input-error").removeClass("input-error");
    }
    else {
        $('#wizard_cell').find("input,textarea,select").attr("disabled", false);
    }
};

function ValidateVPN() {
    var isValid = true;
    var ConnectionName = $("#ConnectionName").val().trim();
    if (ConnectionName === '') {
        isValid = false;
    }
    var ConnectionType = $("#ConnectionType").val().trim();
    if (ConnectionType === 'L2TP') {
        var L2TP_Server = $("#L2TP_Server").val().trim();
        if (L2TP_Server === '') {
            isValid = false;
        }

        var L2TP_Account = $("#L2TP_Account").val().trim();
        if (L2TP_Account === '') {
            isValid = false;
        }

        var L2TP_UserAuthentication_RSASecurID = $("#L2TP_UserAuthentication_RSASecurID").val().trim();
        if (L2TP_UserAuthentication_RSASecurID === 'Password') {
            var L2TP_UserAuthentication_Password = $("#L2TP_UserAuthentication_Password").val().trim();
            if (L2TP_UserAuthentication_Password === '') {
                isValid = false;
            }
        }

        var L2TP_ProxySetup = $("#L2TP_ProxySetup").val().trim();
        if (L2TP_ProxySetup === 'Manual') {
            var L2TP_ProxySetup_Server = $("#L2TP_ProxySetup_Server").val().trim();
            if (L2TP_ProxySetup_Server === '') {
                isValid = false;
            }
        }
    }
    else {
        var IPSEC_SERVER = $("#IPSEC_SERVER").val().trim();
        if (IPSEC_SERVER === '') {
            isValid = false;
        }

        var IPSEC_Account = $("#IPSEC_Account").val().trim();
        if (IPSEC_Account === '') {
            isValid = false;
        }

        var IPSEC_Account_Password = $("#IPSEC_Account_Password").val().trim();
        if (IPSEC_Account_Password === '') {
            isValid = false;
        }

        var IPSEC_ProxySetup = $("#IPSEC_ProxySetup").val().trim();
        if (IPSEC_ProxySetup === 'Manual') {
            var IPSEC_Proxy_Server = $("#IPSEC_Proxy_Server").val().trim();
            if (IPSEC_Proxy_Server === '') {
                isValid = false;
            }
        }
    }

    if (!isValid) {
        disableLink('lBVPN');
    } else {
        enableLink('lBVPN');
    }
}