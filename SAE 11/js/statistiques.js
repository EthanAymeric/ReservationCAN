// tableau identifiant de liaison : libellé
let tableLiaison =
{
    "1" : "Lorient-Groix",
    "2" : "Groix-Lorient",
    "3" : "Quiberon-Belle-Ile",
    "4" : "Belle-Ile-Quiberon"
}

let liaison = document.getElementById("liaison-select-stats"); // Select permettant de choisir une liaison

for (let id in tableLiaison) // Boucle sur toutes les liaisons pour les afficher dans le select
{
    liaison.innerHTML+=`<option value="${id}">${id} : ${tableLiaison[id]}</option>`;
}

let fetchAPIliaison = async () => {
    {
        try
        {
            const containerStatistiques = document.getElementById('containerStatistiques'); // Récupération du container
            containerStatistiques.innerHTML=""; // Réinitialisation de l'affichage
            let id=liaison.value; // Récupération de l'id
            const response = await fetch (`https://can.iutrs.unistra.fr/api/liaison/${id}/chiffreAffaire`); //fetch API
            if (!response.ok){
                throw new Error ('erreur de reseau !');
            }
            let data = await response.json();

            let total = data.passagers.chiffreAffaire + data.vehicules.chiffreAffaire; // Calcul du chiffre d'affaires total

            let codeHTML = `<br>
                            <h3>Liaison : ${tableLiaison[id]}</h3>
                            <br>
                            <table class="chiffreAffaire">
                            <tr>
                                <th>Chiffre d'affaires passagers</th>
                                <th>Nombre de passagers</th>
                                <th>Chiffre d'affaires véhicules</th>
                                <th>Nombre de Véhicules</th>
                                <th>Total chiffre d'affaire</th>
                            </tr>
                            <tr>
                                <td>${data.passagers.chiffreAffaire.toFixed(2)} €</td>
                                <td>${data.passagers.nombre}</td>
                                <td>${data.vehicules.chiffreAffaire.toFixed(2)} €</td>
                                <td>${data.vehicules.quantite}</td>
                                <td>${total.toFixed(2)} €</td> 
                            </tr>
                            </table>`;

        containerStatistiques.innerHTML = codeHTML; // Ajout html dans container
        }
        catch (error){
            console.error("Une erreur est survenue :", error);
        }

    }
}

liaison.addEventListener("click",fetchAPIliaison); //Fonction statistiques quand on clique
liaison.addEventListener("change",fetchAPIliaison); //Fonction statistiques quand on change de liaison