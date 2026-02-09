// ===== hämta element =====
// Hämtar formuläret och meddelanderutan från HTML så de kan användas i JS
let form = document.getElementById("registerForm");
let message = document.getElementById("message");

// ===== email domain extra =====
// Hämtar dropdown för e-postdomän och fältet för egen domän
let domainSelect = document.getElementById("emailDomain");
let customDomain = document.getElementById("customDomain");

// Körs när domänvalet ändras
domainSelect.addEventListener("change", function () {
    // Om användaren väljer "other", visa extra inputfält
    if (domainSelect.value === "other") {
        customDomain.classList.remove("hidden");
    } else {
        // Annars dölj extra fältet
        customDomain.classList.add("hidden");
    }
});

// ===== visa lösenord =====
// Hämtar checkbox och lösenordsfälten
let showBox = document.getElementById("showPassword");
let pass1 = document.getElementById("password");
let pass2 = document.getElementById("repeatPassword");

// Körs när checkboxen ändras
showBox.addEventListener("change", function () {
    // Växlar typ mellan text och password
    let type = showBox.checked ? "text" : "password";
    pass1.type = type;
    pass2.type = type;
});

// ===== villkor modal =====
// Hämtar modalruta och knappar för att öppna och stänga
let modal = document.getElementById("termsModal");
let openBtn = document.getElementById("openTerms");
let closeBtn = document.getElementById("closeTerms");

// Öppnar villkorsrutan när länken klickas
openBtn.onclick = function () {
    modal.classList.add("active");
};

// Stänger rutan när stäng-knappen klickas
closeBtn.onclick = function () {
    modal.classList.remove("active");
};

// Stänger rutan om man klickar på bakgrunden utanför innehållet
modal.onclick = function (e) {
    if (e.target === modal) {
        modal.classList.remove("active");
    }
};

// ===== validering =====
// Körs när formuläret skickas
form.addEventListener("submit", function (e) {
    e.preventDefault(); // Stoppar sidladdning vid submit

    // Hämtar och rensar textvärden från inputfält
    let first = document.getElementById("firstName").value.trim();
    let last = document.getElementById("lastName").value.trim();
    let age = Number(document.getElementById("age").value);

    // Hämtar e-postens namn-del
    let emailName = document.getElementById("emailName").value.trim();

    // Bestämmer domän, antingen vald eller egen inmatad
    let domain = domainSelect.value === "other"
        ? customDomain.value.trim()
        : domainSelect.value;

    // Hämtar valt program och om villkor är accepterade
    let program = document.getElementById("program").value;
    let terms = document.getElementById("terms").checked;

    // Kontroll att förnamn och efternamn finns
    if (!first || !last) {
        message.textContent = "Fyll i namn";
        return; // Avbryt om fel
    }

    // Kontroll att ålder är inom tillåtet intervall
    if (age < 18 || age > 30) {
        message.textContent = "Ålder måste vara 18–30";
        return;
    }

    // Kontroll att e-postens delar är ifyllda
    if (!emailName || !domain) {
        message.textContent = "E-post saknas";
        return;
    }

    // Kontroll att lösenord har minsta längd
    if (pass1.value.length < 6) {
        message.textContent = "Lösenord minst 6 tecken";
        return;
    }

    // Kontroll att båda lösenorden matchar
    if (pass1.value !== pass2.value) {
        message.textContent = "Lösenord matchar inte";
        return;
    }

    // Kontroll att program är valt
    if (!program) {
        message.textContent = "Välj program";
        return;
    }

    // Kontroll att villkoren är accepterade
    if (!terms) {
        message.textContent = "Acceptera villkoren";
        return;
    }

    // Om alla kontroller passerar, visa bekräftelse
    message.textContent = "Konto skapat";
});
