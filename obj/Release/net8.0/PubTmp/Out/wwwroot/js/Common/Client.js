var countryList = @Html.Raw(System.Text.Json.JsonSerializer.Serialize(ViewBag.countrylist));
var ownerlist = @Html.Raw(System.Text.Json.JsonSerializer.Serialize(ViewBag.ownerlist));

$(document).ready(function () {
    InitializeStates();
    var ClientID = $('#ClientID').val();
    if (ClientID > 0) {
        var editData = @Html.Raw(Json.Serialize(Model));
        PopulateEditForm(editData);
    }

});


function PopulateEditForm(d) {
    console.log(JSON.stringify(d));
    if (!d) return;

    $('#CompanyID').val(d.companyID);
    $('input[name="ClientName"]').val(d.clientDetails.clientName);
    $('input[name="PANNo"]').val(d.clientDetails.panNo);
    $('input[name="TINNO"]').val(d.clientDetails.tinno);
    $('input[name="VATNO"]').val(d.clientDetails.vatno);
    $('input[name="CSTNO"]').val(d.clientDetails.cstno);
    $('input[name="STNO"]').val(d.clientDetails.stno);
    $('input[name="ECCNO"]').val(d.clientDetails.eccno);
    $('input[name="GSTNo"]').val(d.clientDetails.gstNo);

    if (d.clientDetails.clientType) {
        $(`input[name="ClientType"][value="${d.clientDetails.clientType}"]`).prop('checked', true);
    }

    $('input[name="TransporterName"]').val(d.logistics.transporterName);
    $('input[name="BookingType"]').val(d.logistics.bookingType);
    $('input[name="ClientTypePermanent"]').val(d.logistics.originalInvoiceCopy);
    $('input[name="FreightType"]').val(d.logistics.freightType);
    $('input[name="BookedTo"]').val(d.logistics.bookedTo);
    $('input[name="PaymentTerms"]').val(d.logistics.paymentTerms);
    $('input[name="CreditPeriod"]').val(d.logistics.creditPeriod);

    $('#IndicateViaMail').prop('checked', d.logistics.indicateViaMail);

    if (d.logistics.exportType) {
        $(`input[name="ExportType"][value="${d.logistics.exportType}"]`).prop('checked', true);
    }

    if (d.logistics.originalInvoiceCopy) {
        $(`input[name="OriginalInvoiceCopy"][value="${d.logistics.originalInvoiceCopy}"]`).prop('checked', true);
    }

    $("#Owner").val(d.logistics.ownerID);
    $('input[name="Email"]').val(d.contactDetails.email);
    $('input[name="AlternateEmail"]').val(d.contactDetails.alternateEmail);
    $('input[name="ContactPersonName"]').val(d.contactDetails.contactPersonName);
    $('input[name="OffMobileNO"]').val(d.contactDetails.offMobileNO);
    $('input[name="WhatsappNo"]').val(d.contactDetails.whatsappNo);
    $('input[name="Website"]').val(d.contactDetails.website);
    $('input[name="Tel1"]').val(d.contactDetails.telephone);
    $('#MobileContainer').find('.mobile-row:not(:first)').remove();

    if (d.contactDetails.mobileNumbers && d.contactDetails.mobileNumbers.length > 0) {
        d.contactDetails.mobileNumbers.forEach((num, index) => {
            if (index === 0) {
                $('input[name="MobileNo"]').first().val(num);
            } else {

                AddMobileRow();
                $('input[name="MobileNo"]').last().val(num);
            }
        });
    }

    bindAddressData("Address", d.companyAddress);
    bindAddressData("Delivery", d.deliveryAddress);
}



function InitializeStates() {
    const dropdowns = ["Address_Country", "Delivery_Country"];
    dropdowns.forEach(id => {
        let dropdown = document.getElementById(id);
        countryList.forEach(state => {
            let option = new Option(state.Name, state.ID);
            dropdown.add(option);
        });
    });

    const dropdowns_Owner = ["Owner"];

    dropdowns_Owner.forEach(id => {
        let dropdown_Owner = document.getElementById(id);
        ownerlist.forEach(owner => {
            let option = new Option(owner.Name, owner.ID);
            dropdown_Owner.add(option);
        });
    });

}


function OnCountryChange(countryid, prefix) {

    var cityDropdown = document.getElementById(prefix + "_State");
    cityDropdown.innerHTML = '<option value="">-- Select _State --</option>';

    if (!countryid) return;

    $.ajax({
        url: '/ArihantERP/Users_Master/GetState',
        type: "GET",
        data: {
            countryid: countryid
        },
        success: function (cityList) {
            cityList.forEach(function (city) {
                cityDropdown.add(new Option(city.name, city.id));
            });
        }
    });
}

function SaveClient() {

    if (!ValidateClientForm()) {
        return;
    }

    var finalData = {

        ClientID: parseInt($('#ClientID').val()) || 0,
        ClientDetails: getClientDetails(),
        ContactDetails: getContactDetails(),
        Logistics: getLogisticsDetails(),
        CompanyAddress: getAddressData("Address"),
        DeliveryAddress: getAddressData("Delivery")
    };

    console.log("Data to Save:", finalData);

    var jsonString = JSON.stringify(finalData);
    $.ajax({
        url: '/ArihantERP/Users_Master/SaveClient',
       
        type: 'POST',

        data: {
            jsonData: jsonString
        },
        success: function (response) {
            if (response.success) {
                Swal.fire('Success', 'Information saved successfully!', 'success')
                    .then(() => {
                        window.location.href = '/ArihantERP/Users_Master/AllClientMasterList';
                           
                    });
            } else {
                Swal.fire('Error', response.message, 'error');
            }
        },
        error: function () {
            Swal.fire('Error', 'An error occurred while saving.', 'error');
        }
    });
}

function ValidateClientForm() {
    var errors = [];
    var client = getClientDetails();
    if (!client.ClientName) errors.push("Client Name is required.");
    if (!client.GSTNo) errors.push("GST No. is required.");


    var taxFields = {
        "TIN NO": client.TINNO,
        "VAT NO": client.VATNO,
        "CST NO": client.CSTNO,
        "ST NO": client.STNO,
        "ECC NO": client.ECCNO
    };
    for (var key in taxFields) {
        if (taxFields[key] && parseFloat(taxFields[key]) < 0) {
            errors.push(key + " cannot be a negative value.");
        }
    }

    var panPattern = /^[A-Z]{5}[0-9]{4}[A-Z]{1}$/;
    if (client.PANNo && !panPattern.test(client.PANNo.toUpperCase())) {
        errors.push("Invalid PAN Number format.");
    }

    var mobileFields = [{
        id: "#OffMobileNo",
        name: "Off. Mobile No."
    },
    {
        id: "#WhatsAppNo",
        name: "WhatsApp No."
    },
    {
        id: "#MobileNo",
        name: "Mobile Number"
    }
    ];

    mobileFields.forEach(function (field) {
        var val = $(field.id).val();
        if (val && val.length !== 10) {
            errors.push(field.name + " must be exactly 10 digits.");
        }
    });

    var prefixes = ["Address", "Delivery"];
    prefixes.forEach(function (p) {
        var addr = getAddressData(p);
        var label = (p === "Address") ? "Company Address" : "Delivery Address";

        if (!addr.AddressLine1) errors.push(label + ": Address Line 1 is required.");
        if (!addr.Pincode) errors.push(label + ": Pincode is required.");
        else if (addr.Pincode.length !== 6) errors.push(label + ": Pincode must be 6 digits.");

        if (!addr.CountryID) errors.push(label + ": Country is required.");
        if (!addr.StateID) errors.push(label + ": State is required.");
        if (!addr.CityID) errors.push(label + ": City is required.");
    });


    var log = getLogisticsDetails();
    if (!log.TransporterName) errors.push("Transport Name is required.");
    if (!log.BookingType) errors.push("Booking Type is required.");
    if (!log.FreightType) errors.push("Freight Type is required.");
    if (!log.BookedTo) errors.push("Booked To is required.");
    if (!log.PaymentTerms) errors.push("Payment Terms are required.");

    if (!log.CreditPeriod) {
        errors.push("Credit Period is required.");
    } else if (parseFloat(log.CreditPeriod) < 0) {
        errors.push("Credit Period cannot be negative.");
    }

    if (!log.OwnerID) errors.push("Please select an Owner.");

    if (!log.ExportType) {
        errors.push("Please select an Export Type (Local or Export).");
    }

    if (!log.IndicateViaMail) {
        errors.push("Please check 'Indicate Via Mail'.");
    }

    if (!log.OriginalInvoiceCopy) {
        errors.push("Please select where the Original Invoice Copy should go (Head Office or Client).");
    }

    if (errors.length > 0) {
        Swal.fire({
            title: 'Validation Failed',
            html: '<div style="text-align:left"><ul><li>' + errors.join('</li><li>') + '</li></ul></div>',
            icon: 'warning'
        });
        return false;
    }

    return true;
}




function getClientDetails() {
    return {
        ClientName: $('#ClientName').val().trim(),
        ClientType: $('input[name="ClientType"]:checked').val(),
        PANNo: $('#PANNo').val(),
        TINNO: $('#TINNO').val(),
        VATNO: $('#VATNO').val(),
        CSTNO: $('#CSTNO').val(),
        STNO: $('#STNO').val(),
        ECCNO: $('#ECCNO').val(),
        GSTNo: $('#GSTNo').val()
    };
}


function getLogisticsDetails() {
    return {
        TransporterName: $('#TransporterName').val(),
        BookingType: $('#BookingType').val(),
        FreightType: $('#FreightType').val(),
        BookedTo: $('#BookedTo').val(),
        PaymentTerms: $('#PaymentTerms').val(),
        CreditPeriod: $('#CreditPeriod').val(),
        ExportType: $('input[name="ExportType"]:checked').val(),
        IndicateViaMail: $('#IndicateViaMail').is(':checked'),
        OriginalInvoiceCopy: $('input[name="OriginalInvoiceCopy"]:checked').val(),
        OwnerID: $('#Owner').val()
    };
}

function getAddressData(prefix) {
    return {
        AddressLine1: $('#' + prefix + '_AddressLine1').val(),
        AddressLine2: $('#' + prefix + '_AddressLine2').val(),
        Pincode: $('#' + prefix + '_Pincode').val(),
        CountryID: $('#' + prefix + '_Country').val(),
        Landmark: $('#' + prefix + '_Landmark').val(),
        StateID: $('#' + prefix + '_State').val(),
        CityID: $('#' + prefix + '_City').val()
    };
}

function getContactDetails() {

    var mobileList = [];
    $('input[name="MobileNo"]').each(function () {
        var val = $(this).val().trim();
        if (val !== "") {
            mobileList.push(val);
        }
    });

    return {
        Email: $('input[name="Email"]').val(),
        AlternateEmail: $('input[name="AlternateEmail"]').val(),
        ContactPersonName: $('input[name="ContactPersonName"]').val(),
        OffMobileNO: $('input[name="OffMobileNO"]').val(),
        WhatsappNo: $('input[name="WhatsappNo"]').val(),
        Website: $('input[name="Website"]').val(),
        Telephone: $('input[name="Tel1"]').val(),
        MobileNumbers: mobileList
    };
}


function bindAddressData(prefix, data) {
    if (!data) return;
    $(`#${prefix}_AddressLine1`).val(data.addressLine1);
    $(`#${prefix}_AddressLine2`).val(data.addressLine2);
    $(`#${prefix}_Pincode`).val(data.pincode);
    $(`#${prefix}_Landmark`).val(data.landmark);
    $(`#${prefix}_Country`).val(data.countryID).trigger('change');
    setTimeout(function () {
        $(`#${prefix}_State`).val(data.stateID).trigger('change');
        setTimeout(function () {
            $(`#${prefix}_City`).val(data.cityID);
        }, 800);
    }, 800);
}