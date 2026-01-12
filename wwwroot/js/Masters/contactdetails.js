function getContactDetails() {
    return {
        Email: $("input[name='Email']").val()?.trim() || "",
        AlternateEmail: $("input[name='AlternateEmail']").val() || "",
        ContactPersonName: $("input[name='ContactPersonName']").val()?.trim() || "",
        OffMobileNO: $("input[name='MobileNo']").val()?.trim() || "", 
        WhatsappNo: $("input[name='WhatsappNo']").val() || "",
        Website: $("input[name='Website']").val() || "",
        Tel1: $("input[name='Tel1']").val() || "",
        MobileNo: $("input[name='MobileNo']").val() || ""
    };
}

function bindContactDetails(email, altEmail, person, offMobile, whatsapp, website, tel, mobile) {
    $("input[name='Email']").val(email);
    $("input[name='AlternateEmail']").val(altEmail);
    $("input[name='ContactPersonName']").val(person);
    $("input[name='OffMobileNO']").val(offMobile);
    $("input[name='WhatsappNo']").val(whatsapp);
    $("input[name='Website']").val(website);
    $("input[name='Tel1']").val(tel);
    $("input[name='MobileNo']").val(mobile);
}