function getAddressTableData() {
    const addressList = [];

    $("#tblAddress tbody tr").each(function () {
        const columns = $(this).find('td');
        if (columns.length > 0) {
            addressList.push({
                AddressLine1: columns.eq(1).text().trim(),
                AddressLine2: columns.eq(2).text().trim(),
                Landmark: columns.eq(3).text().trim(),
                Pincode: columns.eq(4).text().trim(),
                GSTNO: columns.eq(8).text().trim(),
                AddressTypeID: columns.eq(10).text().trim(),
                CountryID: columns.eq(11).text().trim(),
                StateID: columns.eq(12).text().trim(),
                CityID: columns.eq(13).text().trim()
            });
        }
    });

    return addressList;
}



function bindDropdownList(elementIds, dataList) {
    elementIds.forEach(id => {
        let dropdown = document.getElementById(id);
        if (dropdown) {
            dropdown.options.length = 1;
            dataList.forEach(item => {
                dropdown.add(new Option(item.Name, item.ID));
            });
        }
    });
}

function bindAddressTable(list) {
    const $tbody = $("#tblAddress tbody").empty();
    if (!list || list.length === 0) return;

    list.forEach(addr => {
        let typeOption = $("#ddlAddressType option[value='" + addr.AddressTypeID + "']");
        let typeName = typeOption.length > 0 ? typeOption.text() : addr.AddressTypeID;

        let row = `<tr>
            <td>${typeName}</td>
            <td>${addr.AddressLine1 || ''}</td>
            <td>${addr.AddressLine2 || ''}</td>
            <td>${addr.Landmark || ''}</td>
            <td>${addr.Pincode || ''}</td>
            <td>${addr.CityName || ''}</td>
            <td>${addr.StateName || ''}</td>
            <td>${addr.CountryName || ''}</td>
            <td>${addr.GSTNO || ''}</td>
            <td>
                <button type="button" class="btn-action btn-edit" onclick="OpenEditModal(this, 'ADDR')">Edit</button>
                <button type="button" class="btn-action btn-remove" onclick="$(this).closest('tr').remove()">Remove</button>
            </td>
            <td style="display:none;">${addr.AddressTypeID}</td>
            <td style="display:none;">${addr.CountryID}</td>
            <td style="display:none;">${addr.StateID}</td>
            <td style="display:none;">${addr.CityID}</td>
        </tr>`;
        $tbody.append(row);
    });
}




function AddAddressToGrid() {
    let p = "MainAddr";
    let modal = $("#modalInputs");
    let typeID = modal.find("#ddlAddressType").val();
    let typeName = modal.find("#ddlAddressType option:selected").text();
    let a1 = modal.find(`#${p}_AddressLine1`).val();
    let a2 = modal.find(`#${p}_AddressLine2`).val();
    let pin = modal.find(`#${p}_Pincode`).val();
    let lnd = modal.find(`#${p}_Landmark`).val();
    let cID = modal.find(`#${p}_Country`).val();
    let cText = modal.find(`#${p}_Country option:selected`).text();
    let sID = modal.find(`#${p}_State`).val();
    let sText = modal.find(`#${p}_State option:selected`).text();
    let ctID = modal.find(`#${p}_City`).val();
    let ctText = modal.find(`#${p}_City option:selected`).text();
    let gstNo = modal.find(`#${p}_GSTNo`).val().trim();

    const gstRegex = /^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$/;
    if (!gstRegex.test(gstNo)) {
        Swal.fire({
            icon: 'error',
            title: 'Validation Error',
            text: 'Invalid Main GST format. Expected: 22AAAAA0000A1Z5',
            target: document.getElementById('editModal')
        });
        return;
    }


    if (!typeID || !a1 || !pin || !cID || !sID || !ctID) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Please fill all mandatory address fields',
            target: document.getElementById('editModal')
        });
        return;
    }


    let isDuplicate = false;
    $("#tblAddress tbody tr").each(function () {
        let existingAccNo = $(this).find("td:eq(8)").text().trim();
        if (existingAccNo === gstNo) {
            isDuplicate = true;
            return false;
        }
    });

    if (isDuplicate) {
        Swal.fire({
            icon: 'warning',
            title: 'Duplicate Entry',
            text: 'This GST Number is already added to the list.',
            target: document.getElementById('editModal')
        });
        return;
    }

    let row = `<tr>
                <td>${typeName}</td>
                <td>${a1}</td>
                <td>${a2}</td>
                <td>${lnd}</td>
                <td>${pin}</td>
                <td>${ctText}</td>
                <td>${sText}</td>
                <td>${cText}</td>
                <td>${gstNo}</td>
                <td>
                    <button type="button" class="btn-action btn-edit" onclick="OpenEditModal(this, 'ADDR')">Edit</button>
                    <button type="button" class="btn-action btn-remove" onclick="$(this).closest('tr').remove()">Remove</button>
                </td>
                <td style="display:none;">${typeID}</td>
                <td style="display:none;">${cID}</td>
                <td style="display:none;">${sID}</td>
                <td style="display:none;">${ctID}</td>
            </tr>`;


    $("#tblAddress tbody").append(row);
    $('#tblAddress').show();
    closeModal();
    $(`#${p}_AddressLine1, #${p}_AddressLine2, #${p}_Pincode, #${p}_Landmark`).val('');
    $(`#${p}_Country, #${p}_State, #${p}_City, #ddlAddressType`).val('').trigger('change');
}




function handleDropdownChange(id, prefix, actionName, targetType) {
    var modal = $("#modalInputs");
    var targetDropdown = modal.find("#" + prefix + "_" + targetType);
    var placeholder = "-- Select " + targetType + " --";
    targetDropdown.html('<option value="">' + placeholder + '</option>');
    if (targetType === "State") {
        modal.find("#" + prefix + "_City").html('<option value="">-- Select City --</option>');
    }

    if (!id) return;


    var fullUrl = window.AppConfig.UserMasterBaseUrl + actionName;
    var params = {};
    var paramKey = (targetType === "State") ? "countryid" : "stateId";
    params[paramKey] = id;

    $.get(fullUrl, params, function (data) {
        if (data && data.length > 0) {
            var options = '';
            data.forEach(function (item) {
                options += '<option value="' + item.id + '">' + item.name + '</option>';
            });
            targetDropdown.append(options);
            console.log(targetType + " appended to #" + prefix + "_" + targetType);
        }
    }).fail(function () {
        Swal.fire('Error', 'Could not load ' + targetType.toLowerCase() + 's', 'error');
    });
}

function getFullUrl(actionName) {
    // Removes trailing slash if exists and adds exactly one
    var base = window.AppConfig.UserMasterBaseUrl.replace(/\/$/, "");
    return base + "/" + actionName;
}


function OnCountryChange(countryid, prefix) {
    var modal = $("#modalInputs");
    var stateDropdown = modal.find("#" + prefix + "_State");
    var cityDropdown = modal.find("#" + prefix + "_City");

    stateDropdown.html('<option value="">-- Select State --</option>');
    cityDropdown.html('<option value="">-- Select City --</option>');

    if (!countryid) return;

    var url = getFullUrl("GetState");
    console.log("Fetching States from:", url);

    $.get(url, { countryid: countryid }, function (data) {
        if (data && data.length > 0) {
            var options = '';
            data.forEach(function (s) {

                options += '<option value="' + (s.id || s.ID) + '">' + (s.name || s.Name) + '</option>';
            });
            stateDropdown.append(options);
        }
    }).fail(function (xhr) {
        console.error("State Load Failed:", xhr.statusText);
    });
}

function OnStateChange(stateId, prefix) {
    var modal = $("#modalInputs");
    var cityDropdown = modal.find("#" + prefix + "_City");

    cityDropdown.html('<option value="">-- Select City --</option>');

    if (!stateId) return;

    var url = getFullUrl("GetCitiesByState");
    console.log("Fetching Cities from:", url);

    $.get(url, { stateId: stateId }, function (data) {
        if (data && data.length > 0) {
            var options = '';
            data.forEach(function (c) {
                options += '<option value="' + (c.id || c.ID) + '">' + (c.name || c.Name) + '</option>';
            });
            cityDropdown.append(options);
        }
    }).fail(function (xhr) {
        console.error("City Load Failed:", xhr.statusText);
    });
}

function populateModalDropdowns(typeID, countryID, stateID, cityID) {
    console.log("--- Populating Modal for Edit ---");
    let modal = $("#modalInputs");

    if (typeID) {
        modal.find("#ddlAddressType").val(typeID);
    }


    if (countryID) {
        modal.find("#MainAddr_Country").val(countryID);

        $.ajax({
            url: getFullUrl("GetState"),
            type: "GET",
            data: { countryid: countryID },
            async: false, 
            success: function (list) {
                let stateDD = modal.find("#MainAddr_State").empty().append('<option value="">--Select State--</option>');
                if (list && list.length > 0) {
                    list.forEach(i => {
                        stateDD.append(new Option(i.name || i.Name, i.id || i.ID));
                    });
                }
                modal.find("#MainAddr_State").val(stateID);
            }
        });
    }


    if (stateID) {
        $.ajax({
            url: getFullUrl("GetCitiesByState"),
            type: "GET",
            data: { stateId: stateID },
            async: false, 
            success: function (list) {
                let cityDD = modal.find("#MainAddr_City").empty().append('<option value="">--Select City--</option>');
                if (list && list.length > 0) {
                    list.forEach(i => {
                        cityDD.append(new Option(i.name || i.Name, i.id || i.ID));
                    });
                }
                modal.find("#MainAddr_City").val(cityID);
            }
        });
    }
}