let boutonRecherche=document.getElementById("boutonRecherche"); // Récupération du bouton Rechercher
let fetchAPIresa = async () => {
    {
        try
        {
            const containerFacture = document.getElementById('containerFacture'); // Récupération du container
            let id=document.getElementById("numResa").value; // Récupération du numéro de réservation saisi par l'utilisateur
            const response = await fetch (`https://can.iutrs.unistra.fr/api/reservation/${id}`); //fetch api de la reservation
            if (!response.ok){
                throw new Error ('erreur de reseau !');
            }
            let data = await response.json();

            containerFacture.innerHTML = ""; /* vider le container */

            //PASSAGER

            //crée une categorie passager avec libellecatégorie, quantité et prix
            const passagerParCategorie = {};
        
                for (let i = 1; i <= data.nbPassagers; i++) { //boucle pour tous les passagers
                const passager = await fetchAPIpassager(i); //appel fonction fetchAPI passager
                const codePassager = passager.libelleCategorie //code correspond au libellé de catégorie du passager i

                //si le code de la catégorie existe : il y a juste a incrémenter la quantité
                //si le code n'existe pas, on l'ajoute

                if (!passagerParCategorie[codePassager]) //crée la catégorie du passager actuel est inexistante
                {
                    passagerParCategorie[codePassager] ={ //création de la catégorie
                        libelle: passager.libelleCategorie, //libellé de la categorie
                        quantite: 0, //pour le moment, aucun passager dans cette catégorie
                        prix: passager.price, //prix de la catégorie du passager
                    }
                }

                passagerParCategorie[codePassager].quantite++; //incrémentation de la quantité de passagers par categorie
            
            }

            let htmlPassagers = ""; //initialiser le contenu htmlPassagers
            let sousTotalPassager = 0; //initialiser le sous-total (par ligne)
            for (let codeCategorie in passagerParCategorie) //pour toutes les catégories dans passagerParCategorie
            {
                const categoriePassager=passagerParCategorie[codeCategorie]; //categorie actuelle
                if (categoriePassager.quantite >0) //si la catégorie n'est pas vide = au moins 1 passager
                {
                const totalLigne = categoriePassager.quantite * categoriePassager.prix; //calcul de la ligne : nombre de passager * prix pour la categorie
                sousTotalPassager += totalLigne; //ajout de la nouvelle ligne calculée a sousTotalPassager
                
                //contenu du tableau passager de la facture
                htmlPassagers += `
                <tr>
                    <td>${categoriePassager.libelle}</td>
                    <td>${categoriePassager.quantite}</td>
                    <td>${categoriePassager.prix.toFixed(2)} €</td>
                    <td>${totalLigne.toFixed(2)} €</td>
                </tr>
                `;
                }   
            }

            //VEHICULES
            

            let htmlVehicules = ""; //initialiser le contenu htmlVehicules
            let sousTotalVehicule = 0; //initialiser le sous-total (par ligne)

                for (let i = 1; i <= data.nbVehicules; i++) { //boucle pour tous les Vehicules
                const vehicules = await fetch(`https://can.iutrs.unistra.fr/api/reservation/${id}/vehicule/${i}`); //fetch API
                const v = await vehicules.json(); //récuperer les infos du fetch
                const totalLigneVehicule = v.quantite * v.prix;  //calcul de la ligne : nombre de Vehicules * prix pour la categorie
                sousTotalVehicule += totalLigneVehicule;  //ajout de la nouvelle ligne calculée a sousTotalVehicules
                //contenu du tableau Vehicules de la facture
                htmlVehicules += `
                <tr>
                    <td>${v.libelle}</td>
                    <td>${v.quantite}</td>
                    <td>${v.prix.toFixed(2)} €</td>
                    <td>${totalLigneVehicule.toFixed(2)} €</td>
                </tr>
                `;
                }   
            

            let prixTotal = sousTotalPassager + sousTotalVehicule; //calcul du prix total

            const date = new Date().toLocaleDateString('fr-FR'); //création d'une date d'édition de la facture

            codeHTML = `
            <header id="facture">
                <img id="imgFacture" src="img/CAN.png" alt="LogoCAN" width="300">
				<h2>Facture</h2>
				<br>
			</header>
            <p>Illkirch le ${date}</p>
            <br>
            <p>Réservation n° : ${id}</p>
            <p>Nom de réservation : ${data.nom}</p>
            <br>
            <p>Traversée : ${data.portDepart} - ${data.portArrivee}</p>
            <p>Date : ${data.date}</p>
            <p>Départ : ${data.heure}</p>
            <p>Bateau : ${data.bateau}</p>
            <br>
            
            <article id="passager">
            <h2>Passagers</h2>
            <br>
            <table class="tarif">
                    <thead> 
                        <tr>
                            <th>Catégorie</th>
                            <th>Nombre</th>
                            <th>Prix unitaire</th>
                            <th>Prix ligne</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${htmlPassagers}
                    </tbody>
                </table>
                </article>
                <br>
                <p class="soustotal">Sous-total personnes : ${sousTotalPassager.toFixed(2)} €</p>
                </article>
                <br>
                <article id="vehicule">
                <h2>Véhicules</h2>
                <br>
                    <table class="tarif">
                            <thead> 
                                <tr>
                                    <th>Catégorie</th>
                                    <th>Quantité</th>
                                    <th>Prix unitaire</th>
                                    <th>Prix ligne</th>
                                </tr>
                            </thead>
                            <tbody>
                                ${htmlVehicules}
                            </tbody>
                        </table>
                </article>
                <br>
                <br>
                <p class="soustotal">Sous-total véhicules : ${sousTotalVehicule.toFixed(2)} €</p>
                <br>
                <p class="total">Prix total : ${prixTotal.toFixed(2)} €</p>
                <br>`

                containerFacture.innerHTML+=codeHTML; //ajout du code dans le container

                containerFacture.classList.add("ouvert"); //changement de class=ouvert pour que le container soit visible
        }
        catch (error){
            console.error("Une erreur est survenue :", error);
        }

    }
}

// Fonction qui récupère un passager via l'API, prend i le numero de passager en paramètre
let fetchAPIpassager = async (i) => {
    {
        try
        {
            let id=document.getElementById("numResa").value; // Récupération du numéro de réservation
            const response = await fetch (`https://can.iutrs.unistra.fr/api/reservation/${id}/passager/${i}`); //fetch api
            if (!response.ok){
                throw new Error ('erreur de passager !');
            }
            let data = await response.json()
            console.log("Passager récupéré :", data);
            return { // Retour des données
                code: data.code,
                libelleCategorie: data.libelleCategorie,
                price: data.price
            };
    }
    
    catch (error){
        console.error("Une erreur est survenue :", error);
    }
    }
}

boutonRecherche.addEventListener("click",fetchAPIresa); //fonction executée quand on clique sur rechercher