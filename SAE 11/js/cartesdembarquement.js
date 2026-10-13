let boutonRecherche=document.getElementById("boutonRecherche"); // Recuperer le bouton rechercher
let boutonAller=document.getElementById("boutonAller"); // Recuperer le bouton aller
let boutonRetour=document.getElementById("boutonRetour"); // Recuperer le bouton retouyr
let boutonSelectionne = boutonAller; // Bouton selectionné par défaut : aller

let j = 1; //index de la carte d'embarquement

let id; // Identifiant de la réservation courante

let idInitial = null; // Identifiant de réservation initial (ne change jamais)

let affichageCarte = async () => {
    {
        try
        { 
            const containerCarte = document.getElementById('containerCarte'); // Récupération du container
			const bouton = document.getElementById("bouton"); // Récupération des boutons
			const numeroCarte = document.getElementById('numeroDeCarte');  // Affichage du numéro de carte
            containerCarte.innerHTML=""; //Vider containerCarte

            id=document.getElementById("numResa").value; // Récupération du numéro de réservation

			if(idInitial===null) // Fixe l'id initial
			{
				idInitial=id;
			}
			
            const response = await fetch (`https://can.iutrs.unistra.fr/api/reservation/${id}`); //fetch api
            if (!response.ok){
                throw new Error ('erreur de réservation !');
            }
            let data = await response.json()
            console.log("Réservation récupérée :", data);

            const nbPassagers = data.nbPassagers; // Nombre de passagers
			const nbVehicules = data.nbVehicules; // Nombre de véhicules
			const total = nbPassagers + nbVehicules; // Nombre de cartes total
        
            if (j<1) // Si l'index est inférieur à 1, on revient à la dernière carte
            {
                j=total;
            }

            if (j>total) // Si l'index dépasse le total, on revient à la première carte
            {
                j=1;
            }

			// Contenu de la partie droite (passager ou véhicule)
			let contenuDroite = "";

			//Passager

            if (j<=nbPassagers) //si index < nombre de passager => passager
            {
				const passager = await fetch(`https://can.iutrs.unistra.fr/api/reservation/${id}/passager/${j}`);
            	const p = await passager.json();

                contenuDroite = `<table class="tabCarte">
                		    		<tbody>
										<tr>
                						    <td>Passager</td>
                						    <td class="qrcodeimg"><img src="img/qrcode.png" alt="qrcode" width=60px></td>
                						</tr>
										<tr>
                						    <td>Nom</td>
                						    <td>${p.nom}</td>
                						</tr>
										<tr>
                						    <td>Prénom</td>
                						    <td>${p.prenom}</td>
                						</tr>
										<tr>
                						    <td>Catégorie</td>
                						    <td>${p.libelleCategorie}</td>
                						</tr>
										<tr>
                						    <td>Prix</td>
                						    <td>${p.price.toFixed(2)} €</td>
                						</tr>
									</tbody>
								</table>`;
			}
			else //si index > nombre passager => véhicule
			{
				const vehiculeIndex = j - nbPassagers; //calcul de l'index du véhicule
				const vehicules = await fetch(`https://can.iutrs.unistra.fr/api/reservation/${id}/vehicule/${vehiculeIndex}`);
            	const v = await vehicules.json();

                contenuDroite = `<table class="tabCarte">
                		    		<tbody>
										<tr>
                							    <td>Véhicule</td>
                							    <td class="qrcodeimg"><img src="img/qrcode.png" alt="qrcode" width=60px></td>
                							</tr>
											<tr>
                							    <td>Catégorie</td>
                							    <td>${v.libelle}</td>
                							</tr>
											<tr>
                							    <td>Quantité</td>
                							    <td>${v.quantite}</td>
                							</tr>
											<tr>
                							    <td>Prix</td>
                							    <td>${v.prix.toFixed(2)} €</td>
                							</tr>
										</tbody>
									</table>`;
			}
			
			codeHTML = `
            <br>
            <section class="carteEmbarquement">
			
			<article class="partieGauche">
					<img src="img/logoCarte.png" alt="LogoCAN" width=100%>
					<div class="texteGauche">
						<table class="tabCarte">
                		    <tbody>
                		        <tr>
                				    <td>Gare de départ</td>
                				    <td>${data.portDepart}</td>
                				</tr>
								<tr>
                				    <td>Gare d'arrivée</td>
                				    <td>${data.portArrivee}</td>
                				</tr>
								<tr>
                				    <td>Date</td>
                				    <td>${data.date}</td>
                				</tr>
								<tr>
                				    <td>Heure de départ</td>
                				    <td>${data.heure}</td>
                				</tr>
								<tr>
                				    <td>Bateau</td>
                				    <td>${data.bateau}</td>
                				</tr>
                		    </tbody>
                		</table>
				</div>
						
			</article>
			<article class="partieDroite">
				<h2>Carte d'embarquement</h2>
				<article class="encadrePartieDroite">
					<div class="texteDroiteHaut">
						<table class="tabCarte">
                		    <tbody>
                		        <tr>
                				    <td>Réservation</td>
                				    <td>${id}</td>
                				</tr>
								<tr>
                				    <td>Nom</td>
                				    <td>${data.nom}</td>
                				</tr>
                		    </tbody>
                		</table>
					</div>
					<div class="texteDroiteBas">
                		${contenuDroite}
					</div>
					
				</article>
			</article>
		</section>`

                containerCarte.innerHTML+=codeHTML; // Ajout dans le container

				numeroCarte.innerHTML=`${j}/${total}`; // Affichage du numéro de carte

                containerCarte.classList.add("ouvert"); // Affichage de la carte
				bouton.classList.add("visible"); // Affichage des boutons

				mettreAJourBouton(); // Mise à jour du bouton sélectionné
            }
        catch (error)
		{
            console.error("Une erreur est survenue :", error);
        }

    }
}

function suivant() {
        j++; //incrémente l'index de carte
        affichageCarte();
    }

function precedent() {
        j--; //décrémente l'index de carte
        affichageCarte();
    }

function aller() {
	if (id > idInitial) {  // Vérifie que l'on ne descend pas en dessous de la réservation initiale
        j = 1; // Réinitialisation de l'index de carte
        id--;  // Décrémentation de l'identifiant de réservation
		document.getElementById("numResa").value = id; //afficher la réservation courante dans l'input
        affichageCarte();
		boutonSelectionne = boutonAller; // Mise à jour du bouton actif
		}
    }

function retour() { // Vérifie que l'on ne dépasse pas la réservation autorisée
		if (id < idInitial + 1) {
			j=1; // Réinitialisation de l'index de carte
			id++;  // Incrémentation de l'identifiant de réservation
			document.getElementById("numResa").value = id; //afficher la réservation courante dans l'input
        	affichageCarte();
			boutonSelectionne = boutonRetour; // Mise à jour du bouton actif
		}
    }

function mettreAJourBouton() { // Mise à jour du bouton sélectionné
    boutonAller.classList.remove("selected");
    boutonRetour.classList.remove("selected");
    boutonSelectionne.classList.add("selected");
}

boutonRecherche.addEventListener("click", () => {
	idInitial = null; //réinitialise à chaque nouvelle recherche
	j = 1; //reviens sur la première carte
	affichageCarte(); //fonction afficher carte
});
boutonGauche.addEventListener("click",precedent); // Navigation entre les cartes
boutonDroite.addEventListener("click",suivant); // Navigation entre les cartes
boutonAller.addEventListener("click",aller); // Navigation entre les réservations
boutonRetour.addEventListener("click",retour); // Navigation entre les réservations