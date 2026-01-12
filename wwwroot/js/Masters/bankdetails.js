
function getBankTableData() {
    const bankList = [];

    $("#tblBank tbody tr").each(function () {
        const columns = $(this).find('td');

        if (columns.length > 0) {
            bankList.push({
                BankName: columns.eq(0).text().trim(),
                AccountNo: columns.eq(1).text().trim(),
                IFSCCode: columns.eq(2).text().trim(),
                AccountType: columns.eq(3).text().trim(),
                AccountHolderName: columns.eq(4).text().trim()
            });
        }
    });

    return bankList;
}


function bindBankTable(list) {
    const $tbody = $("#tblBank tbody").empty();
    if (!list || list.length === 0) return;

    list.forEach(bank => {
        let row = `<tr>
            <td>${bank.BankName || ''}</td>
            <td>${bank.AccountNo || ''}</td>
            <td>${bank.IFSCCode || ''}</td>
            <td>${bank.AccountType || ''}</td>
            <td>${bank.AccountHolderName || ''}</td>
            <td>
                <button type="button" class="btn-action btn-edit" onclick="OpenEditModal(this, 'BANK')">Edit</button>
                <button type="button" class="btn-action btn-remove" onclick="$(this).closest('tr').remove()">Remove</button>
            </td>
            <td style="display:none;">${bank.AddressLine1 || ''}</td>
            <td style="display:none;">${bank.AddressLine2 || ''}</td>
            <td style="display:none;">${bank.Pincode || ''}</td>
            <td style="display:none;">${bank.Landmark || ''}</td>
            <td style="display:none;">${bank.CountryID}</td>
            <td style="display:none;">${bank.StateID}</td>
            <td style="display:none;">${bank.CityID}</td>
        </tr>`;
        $tbody.append(row);
    });
}


function AddBankToGrid() {
    let modal = $("#modalInputs");
    let bName = modal.find("input[name='BankName']").val().trim();
    let holder = modal.find("input[name='AccountHolderName']").val().trim();
    let accType = modal.find("select[name='AccountType']").val();
    let accNo = modal.find("input[name='AccountNo']").val().trim();
    let ifsc = modal.find("input[name='IFSCCode']").val().trim().toUpperCase();

    if (!bName || !holder || !accType) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'All bank fields are mandatory',
            target: document.getElementById('editModal')
        });
        return;
    }

    if (!accNo || isNaN(accNo) || parseInt(accNo) < 0 || !Number.isInteger(Number(accNo))) {

        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Account Number must be a valid positive integer',
            target: document.getElementById('editModal')
        });
        return;
    }


    let ifscRegex = /^[A-Z]{4}0[A-Z0-9]{6}$/;
    if (!ifsc) {

        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'IFSC Code is mandatory',
            target: document.getElementById('editModal')
        });

        return;
    } else if (!ifscRegex.test(ifsc)) {

        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Invalid IFSC format. Example: SBIN0001234',
            target: document.getElementById('editModal')
        });

        return;
    }

    let isDuplicate = false;

    $("#tblBank tbody tr").each(function () {
        let existingAccNo = $(this).find("td:eq(1)").text().trim();
        if (existingAccNo === accNo) {
            isDuplicate = true;
            return false;
        }
    });

    if (isDuplicate) {
        Swal.fire({
            icon: 'warning',
            title: 'Duplicate Entry',
            text: 'This Account Number is already added to the list.',
            target: document.getElementById('editModal')
        });
        return;
    }


    $('#AddBankList').hide();
    $('#BankDiv').hide();
    $('#AddBank').show();
    $('#tblBank').show();

    let row = `<tr>
                        <td>${bName}</td>
                        <td>${accNo}</td>
                        <td>${ifsc}</td>
                        <td>${accType}</td>
                        <td>${holder}</td>
                        <td>
                            <button type="button" class="btn-action btn-edit" onclick="OpenEditModal(this, 'BANK')">Edit</button>
                            <button type="button" class="btn-action btn-remove" onclick="$(this).closest('tr').remove()">Remove</button>
                        </td>
                    </tr>`;


    $("#tblBank tbody").append(row);
    closeModal();
    $("input[name='BankName'], input[name='AccountNo'], input[name='IFSCCode'], input[name='AccountHolderName']").val('');
    $("select[name='AccountType']").val('');
}



function OpenEditModal(btn, type) {
    currentRow = $(btn).closest('tr');
    editType = type;
    let cells = currentRow.find('td');
    $("#btnSaveUpdate").show().text("Update Details");
    $("#btnModalAddBank").remove();
    $("#btnModalAddAddr").remove();

    if (type === 'ADDR') {
        $("#modalHeading").text("Edit Address");

        $("#modalInputs").html($('#MAinAddDive').html());
        let modal = $("#modalInputs");
        modal.find("#ddlAddressType").val(cells.eq(9).text());
        modal.find("#MainAddr_AddressLine1").val(cells.eq(1).text());
        modal.find("#MainAddr_AddressLine2").val(cells.eq(2).text());
        modal.find("#MainAddr_Landmark").val(cells.eq(3).text());
        modal.find("#MainAddr_Pincode").val(cells.eq(4).text());
        modal.find("#MainAddr_GSTNo").val(cells.eq(8).text());

        populateModalDropdowns(
            cells.eq(10).text(),
            cells.eq(11).text(),
            cells.eq(12).text(),
            cells.eq(13).text()
        );
    } else {
        $("#modalHeading").text("Edit Bank & Branch");
        $("#modalInputs").html($('#BankDiv').html());
        let modal = $("#modalInputs");
        modal.find("input[name='BankName']").val(cells.eq(0).text());
        modal.find("input[name='AccountNo']").val(cells.eq(1).text());
        modal.find("input[name='IFSCCode']").val(cells.eq(2).text());
        modal.find("select[name='AccountType']").val(cells.eq(3).text());
        modal.find("input[name='AccountHolderName']").val(cells.eq(4).text());
    }

    $("#editModal").show();
}

$("#btnSaveUpdate").click(function () {
    if (!currentRow) return;
    let cells = currentRow.find('td');
    let modal = $("#modalInputs");

    let p = "MainAddr";
    const showModalError = (msg) => {
        Swal.fire({
            icon: 'error',
            title: 'Missing Information',
            text: msg,
            target: document.getElementById('editModal'),
            confirmButtonColor: '#000080'
        });
    };

    let pinRegex = /^[1-9][0-9]{5}$/;

    if (editType === 'ADDR') {
        let typeID = modal.find("#ddlAddressType").val();
        let typeName = modal.find("#ddlAddressType option:selected").text();
        let a1 = modal.find(`#${p}_AddressLine1`).val().trim();
        let a2 = modal.find(`#${p}_AddressLine2`).val().trim();
        let pin = modal.find(`#${p}_Pincode`).val().trim();
        let lnd = modal.find(`#${p}_Landmark`).val().trim();
        let cID = modal.find(`#${p}_Country`).val();
        let cText = modal.find(`#${p}_Country option:selected`).text();
        let sID = modal.find(`#${p}_State`).val();
        let sText = modal.find(`#${p}_State option:selected`).text();
        let ctID = modal.find(`#${p}_City`).val();
        let ctText = modal.find(`#${p}_City option:selected`).text();
        let gstNo = modal.find(`#${p}_GSTNo`).val().trim();

        if (!typeID) {
            showModalError('Address Type is mandatory');
            return;
        }
        if (!a1) {
            showModalError('Address Line 1 is mandatory');
            return;
        }
        if (!pin || !pinRegex.test(pin)) {
            showModalError('Please enter a valid 6-digit Pincode');
            return;
        }
        if (!cID || !sID || !ctID) {
            showModalError('Country, State, and City are mandatory');
            return;
        }

        cells.eq(0).text(typeName);
        cells.eq(1).text(a1);
        cells.eq(2).text(a2);
        cells.eq(3).text(lnd);
        cells.eq(4).text(pin);
        cells.eq(5).text(ctText);
        cells.eq(6).text(sText);
        cells.eq(7).text(cText);
        cells.eq(8).text(gstNo);
        let actionHtml = `
                    <button type="button" class="btn-action btn-edit" onclick="OpenEditModal(this, 'ADDR')">Edit</button>
                    <button type="button" class="btn-action btn-remove" onclick="$(this).closest('tr').remove()">Remove</button>
                `;
        cells.eq(9).html(actionHtml);
        cells.eq(10).text(typeID);
        cells.eq(11).text(cID);
        cells.eq(12).text(sID);
        cells.eq(13).text(ctID);

    } else {

        let bName = modal.find("input[name='BankName']").val().trim();
        let accNo = modal.find("input[name='AccountNo']").val().trim();
        let ifsc = modal.find("input[name='IFSCCode']").val().trim().toUpperCase();
        let accType = modal.find("select[name='AccountType']").val();
        let holder = modal.find("input[name='AccountHolderName']").val().trim();

        if (!bName || !accNo || !ifsc || !holder) {
            Swal.fire('Error', 'All bank fields are mandatory', 'error');
            return;
        }

        cells.eq(0).text(bName);
        cells.eq(1).text(accNo);
        cells.eq(2).text(ifsc);
        cells.eq(3).text(accType);
        cells.eq(4).text(holder);
    }

    closeModal();

});


