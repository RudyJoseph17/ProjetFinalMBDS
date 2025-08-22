window.addEventListener('load', function () {
    console.log('chart-pie-demo.js charg�, test du canvas�');
    var canvas = document.getElementById('myPieChart');
    console.log('canvas trouv� ? ?', canvas);
    if (!canvas) return;  // pas de canvas, on stoppe

    // Exemple : vos donn�es viendront peut-�tre d�une variable JS g�n�r�e par Razor
    // ou d�un appel AJAX. Ici on simule un tableau vide pour l�instant :
    var chartData = {
        labels: [],      // aucun label pour l�instant
        datasets: []     // aucun dataset pour l�instant
    };

    // Si pas de donn�es, on affiche juste le message �noDataMessage� et on arr�te.
    if (!chartData.datasets.length) {
        var msg = document.getElementById('noDataMessage');
        if (msg) msg.style.display = 'block';
        return;
    }

    // Sinon, on cache le message et on construit le chart
    var msg = document.getElementById('noDataMessage');
    if (msg) msg.style.display = 'none';

    var ctx = canvas.getContext('2d');
    new Chart(ctx, {
        type: 'pie',
        data: chartData,
        options: { /* vos options */ }
    });
});
