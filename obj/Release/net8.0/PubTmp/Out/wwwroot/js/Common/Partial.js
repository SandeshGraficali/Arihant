
function bindIdentityDetails(data) {
    $("#companyID").val(data.CompanyID || 0);
    $("input[name='CompanyName']").val(data.CompanyName);
    $("input[name='CEegistration']").val(data.Central_Excise_Registration);
    $("input[name='CERange']").val(data.Central_Excise_Range);
    $("input[name='TINNO']").val(data.TIN_NO);
    $("input[name='VATNO']").val(data.VAT_NO);
    $("input[name='CSTNO']").val(data.CSTNO);
    $("input[name='STNO']").val(data.STNO);
    $("input[name='ECCNO']").val(data.ECCNO);
    $("input[name='GSTNo']").val(data.GSTNo);
    $("input[name='CompanyemailID']").val(data.CompanyemailID);



}
function SelectedValueCountryDropdown(prefix, value) {
    $(`#${prefix}_Country`).val(value);
}
function bindContactCommunication(data) {
    $("input[name='Email']").val(data.Email);

    $("input[name='AlternateEmail']").val(data.AlternetEmail || "");
    $("input[name='ContactPersonName']").val(data.ContactPersonName || "");
    $("input[name='OffMobileNO']").val(data.OfficeMobileNO || "");
    $("input[name='WhatsappNo']").val(data.WhatsAppNO || "");

    $("input[name='Website']").val(data.Website);
    $("input[name='Tel1']").val(data.Telephone);
    $("input[name='MobileNo']").first().val(data.MobileNumber);


    const extraMobiles = [data.MobileNumber2, data.MobileNumber3, data.MobileNumber4, data.MobileNumber5];
    extraMobiles.forEach(mob => {
        if (mob && mob.toString().trim() !== "") {
            AddMobileRow();
            $("input[name='MobileNo']").last().val(mob);
        }
    });
}

function bindBankDetails(data) {
    $("input[name='BankName']").val(data.BankName);
    $("input[name='AccountNo']").val(data.AccountNo);
    $("input[name='AccountHolderName']").val(data.AccountHolderName);
    $("select[name='AccountType']").val(data.AccountType); // Use select for dropdown
    $("input[name='IFSCCode']").val(data.IFSCCode);


    $(`#B_AddressLine1`).val(data.Bank_Address1);
    $(`#B_AddressLine2`).val(data.Bank_Address2);
    $("#B_Landmark").val(data.Bank_Landmark);
    $("#B_Pincode").val(data.Bank_PinCode);
}

function bindAddressTextFields(prefix, addr1, addr2, landmark, pincode) {
    $(`#${prefix}_AddressLine1`).val(addr1);
    $(`#${prefix}_AddressLine2`).val(addr2);
    $(`#${prefix}_Landmark`).val(landmark);
    $(`#${prefix}_Pincode`).val(pincode);
}
function SelectedValueStateDropdawon(prefix, value) {

    let p = prefix;
    $(`#${p}_State`).val(value);
}



function BindDropdown(prefix, stateId, selectedCityId) {

    return new Promise((resolve) => {
        if (!stateId) { resolve(); return; }

        $.ajax({
            url: '/ArihantERP/Users_Master/GetCitiesByState',
            type: "GET",
            data: { stateId: stateId },
            success: function (cityList) {
                var cityDropdown = document.getElementById(prefix + "_City");
                cityDropdown.innerHTML = '<option value="">-- Select City --</option>';

                cityList.forEach(function (city) {
                    var option = new Option(city.name, city.id);
                    cityDropdown.add(option);
                });

                $(cityDropdown).val(selectedCityId);
                resolve();
            },
            error: function () {
                console.error("Error loading cities for " + prefix);
                resolve();
            }
        });
    });
}


function BindDropdownState(prefix, countryid, selectedCityId) {

    return new Promise((resolve) => {
        if (!countryid) { resolve(); return; }

        $.ajax({
            url: '/ArihantERP/Users_Master/GetState',
            type: "GET",
            data: { countryid: countryid },
            success: function (cityList) {
                var cityDropdown = document.getElementById(prefix + "_State");
                cityDropdown.innerHTML = '<option value="">-- Select State --</option>';

                cityList.forEach(function (city) {
                    var option = new Option(city.name, city.id);
                    cityDropdown.add(option);
                });

                $(cityDropdown).val(selectedCityId);
                resolve();
            },
            error: function () {
                console.error("Error loading cities for " + prefix);
                resolve();
            }
        });
    });
}


function AddMobileRow() {

    var rowCount = $('.mobile-row').length;
    if (rowCount >= 5) {
        Swal.fire({
            icon: 'warning',
            title: 'Limit Reached',
            text: 'You can only add a maximum of 4 mobile numbers.',
            confirmButtonColor: '#000080'
        });
        return;
    }

    var html = `
            <div class="col-lg-4 floatleft padding10 mobile-row">
                <label>Mobile Number</label>
                <div style="display: flex; gap: 5px;">
                    <input type="text" name="MobileNo" class="TextBox col-lg-10 BorderRounded5" placeholder="Additional No.">
                    <button type="button" class="Button ButtonColor3" onclick="$(this).closest('.mobile-row').remove()"
                            style="display: flex; align-items: center; justify-content: center; width: 32px; height: 32px; border-radius: 50%; padding: 0; border: none;">
                        <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" fill="white" viewBox="0 0 16 16">
                            <path d="M4 8a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7A.5.5 0 0 1 4 8z" stroke="white" stroke-width="1"/>
                        </svg>
                    </button>
                </div>
            </div>`;

    $('#MobileContainer').append(html);
}

function OnStateChange(stateId, prefix) {
    var cityDropdown = document.getElementById(prefix + "_City");
    cityDropdown.innerHTML = '<option value="">-- Select City --</option>';

    if (!stateId) return;

    $.ajax({
        url: '/ArihantERP/Users_Master/GetCitiesByState',
        type: "GET",
        data: { stateId: stateId },
        success: function (cityList) {
            cityList.forEach(function (city) {
                cityDropdown.add(new Option(city.name, city.id));
            });
        }
    });
}


function getIdentityDetails() {
    return {
        CompanyID: $("#companyID").val(),
        CompanyName: $("input[name='CompanyName']").val().trim(),
        CEegistration: $("input[name='CEegistration']").val().trim(),
        CERange: $("input[name='CERange']").val().trim(),
        TINNO: $("input[name='TINNO']").val().trim(),

        VATNO: $("input[name='VATNO']").val().trim(),
        CSTNO: $("input[name='CSTNO']").val().trim(),
        STNO: $("input[name='STNO']").val().trim(),
        ECCNO: $("input[name='ECCNO']").val().trim(),
        GSTNo: $("input[name='GSTNo']").val().trim(),
        CompanyemailID: $("input[name='CompanyemailID']").val().trim()

    };
}

function getContactCommunication() {
    return {
        Email: $("input[name='Email']").val().trim(),
        AlternateEmail: $("input[name='AlternateEmail']").val().trim(),
        ContactPersonName: $("input[name='ContactPersonName']").val().trim(),
        OffMobileNO: $("input[name='OffMobileNO']").val().trim(),
        WhatsappNo: $("input[name='WhatsappNo']").val().trim(),
        Website: $("input[name='Website']").val().trim(),
        Telephone: $("input[name='Tel1']").val(),
        MobileNumbers: $("input[name='MobileNo']").map(function () {
            return $(this).val();
        }).get().filter(v => v !== "")
    };
}

function getAddressDetails(prefix) {
    return {
        AddressLine1: $(`#${prefix}_AddressLine1`).val(),
        AddressLine2: $(`#${prefix}_AddressLine2`).val(),
        Landmark: $(`#${prefix}_Landmark`).val(),
        Country: $(`#${prefix}_Country`).val(),
        Pincode: $(`#${prefix}_Pincode`).val(),
        City: $(`#${prefix}_City`).val(),
        State: $(`#${prefix}_State`).val()
    };
}

function getBankDetails() {
    return {
        BankName: $("input[name='BankName']").val(),
        AccountNo: $("input[name='AccountNo']").val(),
        AccountHolderName: $("input[name='AccountHolderName']").val(),
        AccountType: $("select[name='AccountType']").val(),
        IFSCCode: $("input[name='IFSCCode']").val()

    };
}