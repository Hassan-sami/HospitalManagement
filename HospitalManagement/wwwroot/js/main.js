
        // Fixed header effect
    window.addEventListener('scroll', function () {
            const header = document.getElementById('header');
            if (window.scrollY > 50) {
        header.classList.add('scrolled');
            } else {
        header.classList.remove('scrolled');
            }
        });

    // Typing effect for welcome message
    const welcomeText = "WELCOME TO OUR HOSPITAL";
    const heroText = document.getElementById("hero-text");
    let index = 0;

    function typeText() {
            if (index < welcomeText.length) {
        heroText.innerHTML += welcomeText.charAt(index);
    index++;
    setTimeout(typeText, 100); // Adjust typing speed
            } else {
        heroText.innerHTML += ""; // Ensure it ends with a blank for styling
            }
        }

        // Start typing effect on load
        window.onload = () => {
        typeText();
        };
