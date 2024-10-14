// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

    function toggleSubButtons(mainMenu) {
            const buttons = mainMenu.querySelectorAll('.button');
            buttons.forEach(button => {
        button.style.display = button.style.display === 'none' || button.style.display === '' ? 'block' : 'none';
            });
        }

    document.addEventListener("DOMContentLoaded", function () {
        const doctorMenu = document.getElementById('doctorMenu');
    const appointmentMenu = document.getElementById('appointmentMenu');
    const patientMenu = document.getElementById('patientMenu');
    const medicalRecordsMenu = document.getElementById('medicalRecordsMenu');

        doctorMenu.addEventListener('click', () => toggleSubButtons(doctorMenu));
            appointmentMenu.addEventListener('click', () => toggleSubButtons(appointmentMenu));
            patientMenu.addEventListener('click', () => toggleSubButtons(patientMenu));
            medicalRecordsMenu.addEventListener('click', () => toggleSubButtons(medicalRecordsMenu));
        });
