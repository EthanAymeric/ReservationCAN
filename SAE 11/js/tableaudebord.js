// tableau identifiant de liaison : libellé
let tableLiaison =
{
    "1" : "Lorient-Groix",
    "2" : "Groix-Lorient",
    "3" : "Quiberon-Belle-Ile",
    "4" : "Belle-Ile-Quiberon"
}

let liaison = document.getElementById("liaison-select"); // Select permettant de choisir une liaison
let date = document.getElementById("date-select"); // Select permettant de choisir une date

for (let id in tableLiaison) // Boucle sur toutes les liaisons pour les afficher dans le select
{
    liaison.innerHTML+=`<option value="${id}">${id} : ${tableLiaison[id]}</option>`;
}

let fetchAPIliaison = async () => {
    {
        try
        {
            const containerTableauDeBord = document.getElementById('containerTableauDeBord'); // Récupération du container
            containerTableauDeBord.innerHTML=""; // Réinitialisation de l'affichage
            let id=liaison.value; // Récupération de l'id
            let dateChoisie = date.value; // Récupération de la date
            const response = await fetch (`https://can.iutrs.unistra.fr/api/liaison/${id}/remplissage/${dateChoisie}`); //fetch API
            if (!response.ok){
                throw new Error ('erreur de reseau !');
            }
            let data = await response.json();

            let codeHTML = `<br>
                            <h3>Liaison : ${tableLiaison[id]} - Date : ${dateChoisie}</h3>
                            <br>`;
            codeHTML += `<table class="taux">
                        <tr>
                            <th>Heure</th>
                            <th>Taux passagers</th>
                            <th>Taux voitures</th>
                        </tr>`;

            

            for (let i = 0; i < data.length; i++)  // Parcours de chaque créneau horaire
                {
                    // Calcul du taux de remplissage des passagers
                    let tauxPassagers = (data[i].nbReservationPassagers * 100) / data[i].capacitePassagers; 
                    // Calcul du taux de remplissage des véhicules
                    let tauxVoitures = (data[i].nbReservationVoitures * 100) / data[i].capaciteVoitures;
                    // Appel de la fonction couleur avec paramètre taux de voitures
                    let couleur = codeCouleur(tauxVoitures);

                    //Dans l'html, on ajoute une classe avec une variable en fonction du taux de remplissage
                    //de voitures pour pouvoir mettre de la couleur en css
                    codeHTML += `<tr>
                                    <td>${data[i].heure}</td>
                                    <td>${tauxPassagers.toFixed(0)}%</td>
                                    <td class="remplissage ${couleur}">${tauxVoitures.toFixed(0)}%</td>
                                </tr>`;
                }

        codeHTML += `</table>`;
        containerTableauDeBord.innerHTML = codeHTML; // Ajout HTML dans le container
        }
        catch (error){
            console.error("Une erreur est survenue :", error);
        }

    }
}

// Fonction permettant d'associer une couleur CSS 
// en fonction du taux de remplissage des véhicules
function codeCouleur(tauxVoitures) {
    if (tauxVoitures <= 50) // Taux inférieur ou égal à 50% : vert
    {
        return "vert";
    }
    if (tauxVoitures <= 75)  // Taux entre 51% et 75% : orange
    {
        return "orange";
    }
    if (tauxVoitures <= 100)
    {
        return "rouge"; // Taux supérieur à 75% : rouge
    }
    else
    {
        return "jaune"; // Taux supérieur à 100% : jaune
    }
}

liaison.addEventListener("click",fetchAPIliaison);
liaison.addEventListener("change", fetchAPIliaison); // Mise à jour lors du changement de liaison
date.addEventListener("change", fetchAPIliaison); // Mise à jour lors du changement de date