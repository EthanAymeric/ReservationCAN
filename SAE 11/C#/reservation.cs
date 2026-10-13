using System;
using System.Text;
using System.IO;
using System.Threading;
using System.Collections.Generic;

class SAE11
{
    static void Main()
    {
        // Déclaration des variables 
        string nomFichier, nomReservation, nomPassager, prenomPassager, categoriePassager, nomLiaison, json, codeCategorie;
        int idLiaisonChoisie, choix, subChoix, choixMenu, choixPassager, choixVehicule, load, totalVehicule, vQuantite; 
        float prixTotal;

        // Dictionnaires contenant les tarifs pour les passagers et les véhicules
        // Initialisation de ces dictionnaires :
        // - la clé représente le code identifiant la catégorie (de passagers ou de véhicules en fonction du dictionnaire)
        // - l'attribut représente les tarifs correspondants (premier élément de la liste : tarif de Groix ; deuxième élément de la liste : tarif de Belle-Île)
        Dictionary<string, List<float>> tarifsPassagers = new Dictionary<string, List<float>>();
        tarifsPassagers.Add("adu26p", new List<float> {18.75f, 18.80f});
        tarifsPassagers.Add("jeu1825", new List<float> {13.80f, 14.10f});
        tarifsPassagers.Add("enf417", new List<float> {11.25f, 11.65f});
        tarifsPassagers.Add("bebe", new List<float> {0f, 0f});
        tarifsPassagers.Add("ancomp", new List<float> {3.35f, 3.35f});

        Dictionary<string, List<float>> tarifsVehicule = new Dictionary<string, List<float>>();
        tarifsVehicule.Add("trot", new List<float> {4.70f, 4.70f});
        tarifsVehicule.Add("velo", new List<float> {8.20f, 8.20f});
        tarifsVehicule.Add("velelec", new List<float> {11.00f, 11.00f});
        tarifsVehicule.Add("cartand", new List<float> {16.45f, 16.45f});
        tarifsVehicule.Add("mobil", new List<float> {23.10f, 23.35f});
        tarifsVehicule.Add("moto", new List<float> {66.05f, 66.40f});
        tarifsVehicule.Add("cat1", new List<float> {96.05f, 98.50f});
        tarifsVehicule.Add("cat2", new List<float> {114.80f, 117.20f});
        tarifsVehicule.Add("cat3", new List<float> {174.45f, 176.90f});
        tarifsVehicule.Add("cat4", new List<float> {210.90f, 213.35f});
        tarifsVehicule.Add("camp", new List<float> {330.20f, 332.70f});

        Dictionary<string, int> Vehicules = new Dictionary<string, int>();

        bool quit, annulation, subQuit, retour;

        Passagers p;
        Reservation r;

        List<Passagers> listePassagers = new List<Passagers>();

        // Programmation
        // Initialisation des variables 
        annulation = false;
        quit = false;
        subQuit = false;
        retour = false;
        
        choix = 0;
        subChoix = 0;
        choixMenu = 0;
        choixPassager = 0;
        choixVehicule = 0;
        prixTotal = 0;

        nomLiaison = "";
        idLiaisonChoisie = 0;
        totalVehicule = 0;
        nomPassager="";
        prenomPassager="";
        vQuantite=0;
        codeCategorie="";

        // ETAPE 1 : Choisir la réservation
        // Saisir le nom de la réservation
        Console.WriteLine("\n\t\t\t\t====== Réservation CAN ======");
        Console.Write("Bienvenue ! \nSaisissez le nom de votre réservation : ");
        nomReservation = Console.ReadLine();
        

        // Choisir la liaison
        do
        {
            Console.WriteLine("\n\t\t\t\t====== Réservation CAN ======");
            Console.WriteLine("\n\t\t\t========= Création de la réservation =========");
            Console.WriteLine("1 - Lorient -> Groix");
            Console.WriteLine("2 - Groix -> Lorient");
            Console.WriteLine("3 - Quiberon -> Le Palais");
            Console.WriteLine("4 - Le Palais -> Quiberon");
            Console.Write("Choix >>> ");
            
            if(!int.TryParse(Console.ReadLine(),out choix)) //Si choix pas int, renvoie 0 pour aller au choix par default
            {
                choix=0;
            }

            // En fonction de la liaison choisie, on ouvrira par la suite le fichier texte contenant tous les horaires correspondants
            switch (choix)
            {
                case 1:
                    nomLiaison = "lgId1.txt";
                    idLiaisonChoisie = 1;
                    quit = true;
                    break;
                
                case 2:
                    nomLiaison = "glId2.txt";
                    idLiaisonChoisie = 2;
                    quit = true;
                    break;

                case 3:
                    nomLiaison = "qpId3.txt";
                    idLiaisonChoisie = 3;
                    quit = true;
                    break;

                case 4:
                    nomLiaison = "pqId4.txt";
                    idLiaisonChoisie = 4;
                    quit = true;
                    break;

                default:
                    Console.WriteLine("Veuillez saisir un nombre entre 1 et 4\n");
                    break;
            }

        }
        while(!quit);

        quit = false;

        // Affiche tout le menu nécessaire pour la création de la réservation
        r = creerReservation(nomLiaison, nomReservation, idLiaisonChoisie);
        

        // ETAPE 2 : Ajout de passagers et de véhicules
        do
        {
            // Choisir si l'on souhaite ajouter un passager, un véhicule ou finaliser la réservation
            Console.WriteLine("\n\t\t\t\t====== Réservation CAN ======");
            Console.WriteLine("\n\t\t\t========= Création de la réservation =========");
            Console.WriteLine("\n1 - Ajouter un passager");
            Console.WriteLine("2 - Ajouter un véhicule");
            Console.WriteLine("3 - Terminer la réservation");
            Console.Write("\nEntrez le nombre correspondant à votre choix >>> ");

            if(!int.TryParse(Console.ReadLine(),out choixMenu)) //Si choixMenu pas int, renvoie 0 pour aller au choix par default
            {
                choixMenu=0;
            }

            switch(choixMenu)
            {
                // Ajouter un nouveau passager
                case 1:
                    
                    // Ajouter sa catégorie
                    do
                    {
                        Console.WriteLine("\nChoisissez la catégorie de passager correspondante : ");
                        Console.WriteLine("\n1 - Adulte 26 ans et plus");
                        Console.WriteLine("2 - Jeune 18 à 25 ans inclus");
                        Console.WriteLine("3 - Enfant 4 à 17 ans inclus");
                        Console.WriteLine("4 - Bébé moins de 4 ans");
                        Console.WriteLine("5 - Animal de compagnie");
                        Console.WriteLine("6 - Annuler");
                        Console.Write("Choix >>> ");

                        if(!int.TryParse(Console.ReadLine(),out choixPassager)) //Si choixPassager pas int, renvoie 0 pour aller au choix par default
                        {
                            choixPassager=0;
                        }

                        if (choixPassager>0 && choixPassager<6)
                        {
                            // Ajouter son nom 
                            Console.Write("\nSaisissez le nom de famille du passager : ");
                            nomPassager = Console.ReadLine();

                            // Ajouter son prénom 
                            Console.Write("\nSaisissez le prénom du passager : ");
                            prenomPassager = Console.ReadLine();
                        }

                        switch (choixPassager)
                        {
                            case 1:
                                
                                categoriePassager = "adu26p";
                                p = new Passagers(nomPassager, prenomPassager, categoriePassager);
                                listePassagers.Add(p);
                                subQuit = true;
                                break;

                            case 2:
                                categoriePassager = "jeu1825";
                                p = new Passagers(nomPassager, prenomPassager, categoriePassager);
                                listePassagers.Add(p);
                                subQuit = true;
                                break;

                            case 3:
                                categoriePassager = "enf417";
                                p = new Passagers(nomPassager, prenomPassager, categoriePassager);
                                listePassagers.Add(p);
                                subQuit = true;
                                break;

                            case 4:
                                categoriePassager = "bebe";
                                p = new Passagers(nomPassager, prenomPassager, categoriePassager);
                                listePassagers.Add(p);
                                subQuit = true;
                                break;
                                
                            case 5:
                                categoriePassager = "ancomp";
                                p = new Passagers(nomPassager, prenomPassager, categoriePassager);
                                listePassagers.Add(p);
                                subQuit = true;
                                break;

                            case 6:
                                
                                subQuit = true;
                                break;
                            
                            default:
                                Console.WriteLine("\nVeuillez choisir un nombre entre 1 et 6");
                                break;
                        }
                    }
                    while(!subQuit);
                    
                    break;

                // Ajouter un nouveau véhicule
                case 2:
                    // Ajouter sa catégorie
                    subQuit = false;
                    do
                    {
                        Console.WriteLine("\nChoisissez la catégorie de véhicule correspondante : ");
                        Console.WriteLine("\n1 - Trotinette électrique");
                        Console.WriteLine("2 - Vélo ou remorque à vélo");
                        Console.WriteLine("3 - Vélo électrique");
                        Console.WriteLine("4 - Vélo cargo ou tandem");
                        Console.WriteLine("5 - Deux-roues <= 125cm³");
                        Console.WriteLine("6 - Deux-roues > 125cm³");
                        Console.WriteLine("7 - Voiture moins de 4m");
                        Console.WriteLine("8 - Voiture de 4m à 4.39m inclus");
                        Console.WriteLine("9 - Voiture de 4.40m à 4.79m inclus");
                        Console.WriteLine("10 - Voiture de 4.80m et plus");
                        Console.WriteLine("11 - Camping-car / Véhicule de plus de 2.10m de haut");
                        Console.WriteLine("12 - Annuler");
                        Console.Write("Choix >>> ");

                        if(!int.TryParse(Console.ReadLine(),out choixVehicule)) //Si choixVehicule pas int, renvoie 0 pour aller au choix par default
                        {
                            choixVehicule=0;
                        }

                        switch (choixVehicule)
                        {
                            case 1:
                                codeCategorie = "trot";
                                
                                subQuit = true;
                                break;

                            case 2:
                                codeCategorie = "velo";
                                
                                subQuit = true;
                                break;
                            
                            case 3:
                                codeCategorie = "velelec";
                                
                                subQuit = true;
                                break;

                            case 4:
                                codeCategorie = "cartand";
                                
                                subQuit = true;
                                break;

                            case 5:
                                codeCategorie = "mobil";
                                
                                subQuit = true;
                                break;

                            case 6:
                                codeCategorie = "moto";
                                
                                subQuit = true;
                                break;

                            case 7:
                                codeCategorie = "cat1";
                                
                                subQuit = true;
                                break;

                            case 8:
                                codeCategorie = "cat2";
                                
                                subQuit = true;
                                break;

                            case 9:
                                codeCategorie = "cat3";
                                
                                subQuit = true;
                                break;

                            case 10:
                                codeCategorie = "cat4";
                                
                                subQuit = true;
                                break;

                            case 11:
                                codeCategorie = "camp";
                                
                                subQuit = true;
                                break;

                            case 12:
                                subQuit = true;
                                break;
                            
                            default:
                                Console.WriteLine("\nVeuillez choisir un nombre entre 1 et 12");
                                break;
                        }

                        while (vQuantite<=0)
                        {
                            Console.Write("\nVeuillez sélectionner la quantité désirée : ");
                            if(!int.TryParse(Console.ReadLine(),out vQuantite)) //Si vQuantite pas int, renvoie 0 pour resaisie
                                {
                                    vQuantite=0;
                                }
                        }

                        if(Vehicules.ContainsKey(codeCategorie)) //Si clé déja présente, ajouter quantité
                        {
                            Vehicules[codeCategorie]+=vQuantite;
                        }
                        else
                        {
                            Vehicules.Add(codeCategorie,vQuantite);
                        }

                        vQuantite=-1;

                    }
                    while(!subQuit);
                    
                    break;
                    
                // Finaliser la réservation
                case 3:
                    Console.WriteLine("Presque fini !");
                    quit = true;
                    break;
                
                // Si on entre un nombre qui n'est pas compris entre 1 et 3...
                default:
                    Console.WriteLine("\nVeuillez entrer un chiffre entre 1 et 3 pour faire votre choix");
                    break;
            }

        }
        while(quit == false);
        

        // ETAPE 3 : Récapitulatif de la réservation effectuée
        // Réinitialisation des variables de menu pour un nouveau menu : celui de la confirmation de la réservation et de son récapitulatif
        choix = 0;
        quit = false;

        
        // Affichage du récapitulatif 
        Console.WriteLine("\n\t\t\t\t====== Réservation CAN ======");
        Console.WriteLine("\nVoici à présent le récapitulatif de votre réservation : ");
        Console.WriteLine("\n\t\t\t====== Récapitulatif ======");
        Console.WriteLine(r.nom);

        // Affichage de la liaison choisie en toutes lettres en fonction de l'idLiaison
        if (r.idLiaison == 1)
        {
            Console.WriteLine("Liaison : Lorient - Groix");
        }
        if (r.idLiaison == 2)
        {
            Console.WriteLine("Liaison : Groix - Lorient");
        }
        if (r.idLiaison == 3)
        {
            Console.WriteLine("Liaison : Quiberon - Belle-Île");
        }
        if (r.idLiaison == 4)
        {
            Console.WriteLine("Liaison : Belle-Île - Quiberon");
        }
        
        Console.WriteLine("Pour le " + r.date + " à " + r.heure);

        // Affichage de chaque passager
        Console.WriteLine("\nNombre de passagers : " + listePassagers.Count);
        Console.WriteLine("\nPassagers : ");
        foreach(Passagers prs in listePassagers)
        {
            // Affichage du nom chaque passager en fonction de sa catégorie 
            Console.WriteLine("Nom : " + prs.nom);
            Console.WriteLine("Prénom : " + prs.prenom);
            Console.Write("Catégorie : ");
            if (prs.codeCategorie == "adu26p")
            {
                Console.Write("Adulte de 26 ans et plus\n");
            }
            if (prs.codeCategorie == "jeu1825")
            {
                Console.Write("Jeune de 18 à 25 ans inclus\n");
            }
            if (prs.codeCategorie == "enf417")
            {
                Console.Write("Enfant de 4 à 17 ans inclus\n");
            }
            if (prs.codeCategorie == "bebe")
            {
                Console.Write("Bébé de moins de 4 ans\n");
            }
            if (prs.codeCategorie == "ancomp")
            {
                Console.Write("Animal de compagnie\n");
            }

            // Affichage du prix en fonction de la liaison choisie         
            if (r.idLiaison == 1 || r.idLiaison == 2)
            {
                Console.WriteLine("Prix : " + tarifsPassagers[prs.codeCategorie][0] + " €");    
                prixTotal = prixTotal + tarifsPassagers[prs.codeCategorie][0];
            }
            else
            {
                Console.WriteLine("Prix : " + tarifsPassagers[prs.codeCategorie][1] + " €");
                prixTotal = prixTotal + tarifsPassagers[prs.codeCategorie][1];
            }
            
            Console.Write("\n");
        }

        // Affichage de chaque véhicule

        //Calculer le nombre total de véhicules
        foreach(string categorieVehicule in Vehicules.Keys)
        {
            totalVehicule += Vehicules[categorieVehicule];
        }

        Console.WriteLine("\nNombre de véhicules : " + totalVehicule);
        Console.WriteLine("\nVéhicules : ");
        foreach(string categorieVehicule in Vehicules.Keys)
        {
            // Affichage du véhicule corresopndant à son code de catégorie ainsi que la quantité réservée
            if (categorieVehicule == "trot")
            {
                Console.WriteLine("Trotinette");
                Console.WriteLine("Quantité : " + Vehicules[categorieVehicule]);
            }
            if (categorieVehicule == "velo")
            {
                Console.WriteLine("Velo ou remorque à vélo");
                Console.WriteLine("Quantité : " + Vehicules[categorieVehicule]);
            }
            if (categorieVehicule == "velelec")
            {
                Console.WriteLine("Vélo électrique");
                Console.WriteLine("Quantité : " + Vehicules[categorieVehicule]);
            }
            if (categorieVehicule == "cartand")
            {
                Console.WriteLine("Vélo cargo ou tandem");
                Console.WriteLine("Quantité : " + Vehicules[categorieVehicule]);
            }
            if (categorieVehicule == "mobil")
            {
                Console.WriteLine("Deux-roues <= 125 cm³");
                Console.WriteLine("Quantité : " + Vehicules[categorieVehicule]);
            }
            if (categorieVehicule == "moto")
            {
                Console.WriteLine("Deux-roues > 125 cm³");
                Console.WriteLine("Quantité : " + Vehicules[categorieVehicule]);
            }
            if (categorieVehicule == "cat1")
            {
                Console.WriteLine("Voiture moins de 4 m");
                Console.WriteLine("Quantité : " + Vehicules[categorieVehicule]);
            }
            if (categorieVehicule == "cat2")
            {
                Console.WriteLine("Voiture de 4 m à 4.39 m");
                Console.WriteLine("Quantité : " + Vehicules[categorieVehicule]);
            }
            if (categorieVehicule == "cat3")
            {
                Console.WriteLine("Voiture de 4.40 m à 4.79 m");
                Console.WriteLine("Quantité : " + Vehicules[categorieVehicule]);
            }
            if (categorieVehicule == "cat4")
            {
                Console.WriteLine("Voiture de plus de 4.80 m");
                Console.WriteLine("Quantité : " + Vehicules[categorieVehicule]);
            }
            if (categorieVehicule == "camp")
            {
                Console.WriteLine("Camping-car - véhicule plus de 2.10 de haut");
                Console.WriteLine("Quantité : " + Vehicules[categorieVehicule]);
            }

            // Affichage du prix en fonction de la liaison choisie et de la quantité demandée
            if (r.idLiaison == 1 || r.idLiaison == 2)
            {
                Console.WriteLine("Prix : " + Vehicules[categorieVehicule] + " x " + tarifsVehicule[categorieVehicule][0] + " = " + Vehicules[categorieVehicule] * tarifsVehicule[categorieVehicule][0] + " €");    
                prixTotal = prixTotal + Vehicules[categorieVehicule] * tarifsVehicule[categorieVehicule][0];
            }
            else
            {
                Console.WriteLine("Prix : " + Vehicules[categorieVehicule] + " x " + tarifsVehicule[categorieVehicule][1] + " = " + Vehicules[categorieVehicule] * tarifsVehicule[categorieVehicule][1] + " €");    
                prixTotal = prixTotal + Vehicules[categorieVehicule] * tarifsVehicule[categorieVehicule][1];
            }
        }


        // Affichage du prix total
        Console.WriteLine("\nPrix total : " + prixTotal + " €");
        
        
        

        // Affichage du menu pour confirmer ou refaire 
        do
        {
            Console.WriteLine("\n1 - Confirmer votre réservation");
            Console.WriteLine("2 - Annuler votre réservation et quitter");
            Console.Write("\nChoix >>> ");
            
            if(!int.TryParse(Console.ReadLine(),out choix)) //Si choix pas int, renvoie 0 pour aller au choix par default
            {
                choix=0;
            }

            switch (choix)
            {
                // Si on confirme, on poursuit normalement
                case 1:
                    Console.WriteLine("\nRéservation confirmée !");
                    quit = true;
                    break;

                // Sinon, on demande à l'utilisateur de bien reconfirmer qu'elle souhaite annuler sa commande 
                case 2:
                    do
                    {
                        Console.WriteLine("\nÊtes-vous sûrs de vouloir annuler votre réservation ?");
                        Console.WriteLine("1 - Oui");
                        Console.WriteLine("2 - Non");
                        Console.Write("Choix >>> ");
                        if(!int.TryParse(Console.ReadLine(),out subChoix)) //Si subChoix pas int, renvoie 0 pour aller au choix par default
                        {
                            subChoix=0;
                        }
                    
                        switch (subChoix)
                        {
                            // Si la personne veut bien annuler, on affiche la réservation
                            case 1:
                                Console.WriteLine("\n\tRéservation annulée !");
                                Console.WriteLine("\tA très bientôt !\n");
                                annulation = true;
                                quit = true;
                                break;

                            // Si la personne ne veut pas annuler, on quitte ce sous-menu pour revenir au menu permettant de confirmer la réservation plus haut
                            case 2:
                                subQuit = true;
                                break;    

                            default:
                                Console.WriteLine("\nVeuillez entrer 1 ou 2 pour faire votre choix");
                                break;
                        }    
                    }
                    while(quit == false && subQuit == false);
                    break;
                    
                    

                default:
                    Console.WriteLine("\nVeuillez entrer 1 ou 2 pour faire votre choix");
                    break;
            }    
        }
        while(quit == false);
        choix = 0;
    
        // ETAPE 4 : Confirmation de la réservation et création du fichier json
        
        // Si la personne n'a pas changé d'avis ni annulé sa réservation
        if (!annulation)
        {
            
            Console.WriteLine("\nSouhaitez-vous réserver des billets retour ?");
            Console.WriteLine("1 - Oui");
            Console.WriteLine("2 - Non");
            Console.Write("Choix >>> ");

            choix = int.Parse(Console.ReadLine());

            if (choix == 1) // Lancer reservation billets retour
            {
                retour = true;  // Booléen utilisé dans la fonction de jsin pour vérifier s'il faut créer une réservation retour dans le fichier
            
            }
            
            // Génération du JSON
            json = genererJSON(r, listePassagers, Vehicules, retour);

            // Construction du nom du fichier
            nomFichier = r.nom.Replace(" ", "_") + "_" + r.date + ".json";

            // Écriture dans le fichier
            File.WriteAllText(nomFichier, json);

            Console.WriteLine("\nRéservation validée !\n");
            
            // Affichage d'une fausse barre de chargement juste parce que c'est marrant :)
            Console.WriteLine("\n\t\t\t\t  LOADING...");
            Console.WriteLine("\t\t     Génération de votre réservation...");
            Console.Write("\t\t\t  [");
                
            for (int i = 0; i <= 21; i++)
            {
                // Imite un temps de chargement plus lent pour la première moitié de l'affichage
                if (i < 12)
                {
                    Random rnd = new Random();
                    load = rnd.Next(150, 350);
                    Console.Write("=");
                    Thread.Sleep(load);
                }
                // Accélère le "chargement" après la moitié de la taille de la barre
                else
                {
                    Console.Write("=");
                    Thread.Sleep(50);
                }
                    
            }
            Console.Write("]");
            Console.WriteLine("\n\n\nMerci de votre réservation et bonne journée !");
        }
        
    }

    // Fonction permettant la génération de la réservation dans un fichier json en suivant les instructions pour celui-ci indiqué dans le sujet
    /* 
    Paramètres : 
        - reservation générale de type Reservation
        - liste de tous les passagers contenant des éléments de type Passagers
        - dictionnaire (clé : chaine de caractères / valeur : entier) 
        - booléen retour vérifiant si on doit générer une réservation retour dans le fichier json
    Valeur de sortie :
        Fichier json contenant la réservation
    */
    static string genererJSON(Reservation res, List<Passagers> listePassagers, Dictionary<string,int> Vehicules, bool retour)
    {
        int compteur = 0;
        int idRetour = 0;
        Reservation r2;
        string nomRetour = "";

        string json = "[\n  {\n";

        // Informations générales sur la réservation
        json += "    \"reservation\": {\n";
        json += "      \"nom\": \"" + res.nom + "\",\n";
        json += "      \"idLiaison\": " + res.idLiaison + ",\n";    
        json += "      \"date\": \"" + res.date + "\",\n";
        json += "      \"heure\": \"" + res.heure + "\",\n";
        json += "      \"horodatage\": \"" + res.horodatage + "\"\n";
        json += "    },\n";

        // Informations sur tous les passagers
        json += "    \"passagers\": [\n";
        for (int i = 0; i < listePassagers.Count; i++)
        {
            json += "      {\n";
            json += "        \"nom\": \"" + listePassagers[i].nom + "\",\n";
            json += "        \"prenom\": \"" + listePassagers[i].prenom + "\",\n";
            json += "        \"codeCategorie\": \"" + listePassagers[i].codeCategorie + "\"\n";
            json += "      }";
            if (i < listePassagers.Count - 1)
            {
                json += ",";
            }
            json += "\n";
        }
        json += "    ],\n";

        // Informations sur tous les véhicules
        json += "    \"vehicules\": [\n";
        foreach (string categorieVehicule in Vehicules.Keys)
        {
            json += "      {\n";
            json += "        \"codeCategorie\": \"" + categorieVehicule + "\",\n";
            json += "        \"quantite\": " + Vehicules[categorieVehicule] + "\n";
            json += "      }";
            if (compteur < Vehicules.Count - 1)
            {
                json += ",";
            }
            json += "\n";
            compteur++;
        }

        json += "    ]\n";
        json += "  }";

        if (retour == false)
        {
            json += "\n]";
        }
        else
        {
            compteur = 0;
            json += ",\n";
            // Si l'utilisateur souhaite créer une réservation retour, on ajoute une réservation qui est en réalité la même mais à l'id "inverse"
            // Ex : Si on a pris la liaison Lorient-Groix (id 1), la liaison retour est Groix-Lorient (id 2) : c'est cet id qui sera affiché sur la réservation retour
            if (res.idLiaison == 1)
            {
                nomRetour = "glId2.txt";
                idRetour = 2;
            }
            if (res.idLiaison == 2)
            {
                nomRetour = "lgId1.txt";
                idRetour = 1;
            }
            if (res.idLiaison == 3)
            {
                nomRetour = "pqId4.txt";
                idRetour = 4;
            }
            if (res.idLiaison == 4)
            {
                nomRetour = "qpId3.txt";
                idRetour = 3;
            }

            Console.WriteLine("\nA présent, saisissez la date et l'horaire de retour souhaités :");

            r2 = creerReservation(nomRetour, res.nom, idRetour);

            // Reservation retour :
            // Informations générales sur la réservation
            json += "  {\n";
            json += "    \"reservation\": {\n";
            json += "      \"nom\": \"" + r2.nom + "\",\n";
            json += "      \"idLiaison\": " + r2.idLiaison + ",\n";    
            json += "      \"date\": \"" + r2.date + "\",\n";
            json += "      \"heure\": \"" + r2.heure + "\",\n";
            json += "      \"horodatage\": \"" + r2.horodatage + "\"\n";
            json += "    },\n";

            // Informations sur tous les passagers
            json += "    \"passagers\": [\n";
            for (int i = 0; i < listePassagers.Count; i++)
            {
                json += "      {\n";
                json += "        \"nom\": \"" + listePassagers[i].nom + "\",\n";
                json += "        \"prenom\": \"" + listePassagers[i].prenom + "\",\n";
                json += "        \"codeCategorie\": \"" + listePassagers[i].codeCategorie + "\"\n";
                json += "      }";
                if (i < listePassagers.Count - 1)
                {
                    json += ",";
                }
                json += "\n";
            }
            json += "    ],\n";

            // Informations sur tous les véhicules
            json += "    \"vehicules\": [\n";
            foreach (string categorieVehicule in Vehicules.Keys)
            {
                json += "      {\n";
                json += "        \"codeCategorie\": \"" + categorieVehicule + "\",\n";
                json += "        \"quantite\": " + Vehicules[categorieVehicule] + "\n";
                json += "      }";
                if (compteur < Vehicules.Count - 1)
                {
                    json += ",";
                }
                json += "\n";
                compteur++;
            }
            json += "    ]\n";
            json += "  }\n]";
        }

        return json;
    }



    // Fonction qui renvoie la structure Reservation et l'initialise avec les valeurs choisies par l'utilisateur
    /*
    Paramètres :
        - chaine de caractères contenant le nom du fichier texte contenant les horaires de la liaison choisie
        - chaine de caractères contenant le nom de la réservation choisi par l'utilisateur
        - entier représentant l'identifiant de la liaison choisie
    Valeur de sortie :
        Reservation de type Reservation
    */
    static Reservation creerReservation(string textLiaison, string nomReservation, int id)
    {
        // Déclaration
        Reservation res;
        string saisie = "";
        string [] temp;
        string [] tab;
        string ligne;
        string horaire;
        string date;

        char[] separateur = new char[] {';'};
        
        bool quitter = false;
        bool subquitter = false;

        int choix = 0;
        int indice = 0;
        int i_saisie =0;
        horaire = "";

        // On propose à l'utilisateur de choisir le jour du mois correspondant
        do 
        {
            Console.Write("\nVeuillez saisir le jour du mois : ");
            saisie = Console.ReadLine();
            if(!int.TryParse(saisie,out i_saisie)) //Si saisie pas int, renvoie 0 pour recommencer une saisie
            {
                i_saisie=0;
            }
        }
        while (i_saisie < 1 || i_saisie > 30); // Novembre = 30 jours

        // Ouverture du fichier correspondant à la liaison choisie
        if (File.Exists(textLiaison)){
            FileStream fs = new FileStream(textLiaison,FileMode.Open, FileAccess.Read);
            StreamReader lesHorrairesId1 = new StreamReader(fs);

            while (!lesHorrairesId1.EndOfStream && quitter == false){

                ligne = lesHorrairesId1.ReadLine();
                temp = ligne.Split(separateur); 
                tab = new string [temp.Length];

                if (saisie == temp[1]){
                    // Affichage d'un menu contenant tous les horaires possibles pour ce jour
                    do {
                    Console.WriteLine("\nVeuillez saisir l'horaire qui vous convient: ");
                    for (int i = 2; i < temp.Length ; i++) //Attention tab[0] et tab[1] sont vides
                    { 
                        tab[i] = temp[i];
                        Console.Write((i-1) + " - " + tab[i] + "\n");
                    }
                    Console.Write("Choix >>> ");

                    if(!int.TryParse(Console.ReadLine(),out choix)) //Si choix n'est pas int, renvoie 0 pour aller au choix par default
                    {
                        choix=0;
                    }

                    // Gestion du choix de l'horaire
                    // Le minimum de choix possible est de 4 horaires
                    // Au maximum, 9 horaires peuvent être sélectionnés
                    // Ainsi, on vérifie pour les choix de 5 à 9 s'ils sont nécessaires ou pas et faire en sorte que la valeur entrée par l'utilisateur marche ou non pour poursuivre la réservation en fonction du nombre de choix
                    switch(choix){
                        case 1 : 
                            indice = 1 + choix;
                            horaire = tab[indice];
                            subquitter = true;
                            break;

                        case 2 : 
                            indice = 1 + choix;
                            horaire = tab[indice];
                            subquitter = true;
                            break;

                        case 3 : 
                            indice = 1 + choix;
                            horaire = tab[indice];
                            subquitter = true;
                            break;

                        case 4 : 
                            indice = 1 + choix;
                            horaire = tab[indice];
                            subquitter = true;
                            break;

                        case 5 : 
                            if (tab.Length > 6){
                                indice = 1 + choix;
                                horaire = tab[indice];
                                subquitter = true;
                            }
                            else{
                                Console.WriteLine("\nCette horaire n'existe pas, réessayez...");
                            }

                            break;

                        case 6 : 
                            if (tab.Length > 7){
                                indice = 1 + choix;
                                horaire = tab[indice];
                                subquitter = true;
                            }
                            else{
                                Console.WriteLine("\nCette horaire n'existe pas, réessayez...");
                            }

                            break;

                        case 7 : 
                            if (tab.Length > 8){
                                indice = 1 + choix;
                                horaire = tab[indice];
                                subquitter = true;
                            }
                            else{
                                Console.WriteLine("\nCette horaire n'existe pas, réessayez...");
                            }

                            break;

                        case 8 : 
                            if (tab.Length > 9){
                                indice = 1 + choix;
                                horaire = tab[indice];
                                subquitter = true;
                            }
                            else{
                                Console.WriteLine("\nCette horaire n'existe pas, réessayez...");
                            }

                            break;

                        case 9 : 
                            if (tab.Length > 10){
                                indice = 1 + choix;
                                horaire = tab[indice];
                                subquitter = true;
                            }
                            else{
                                Console.WriteLine("\nCette horaire n'existe pas, réessayez...");
                            }

                            break;

                        default:
                            Console.WriteLine("\nCette horaire n'existe pas, réessayez...");
                            break;
                    }
                    }
                    while(!subquitter); 
                        quitter = true;
                    }   
                }
            lesHorrairesId1.Close();
        }
        Console.Write("\n");

        // Gère la date du système
        if (i_saisie < 10)
        {
            date = "2025-11-0" + saisie;        
        }
        else
        {
            date = "2025-11-" + saisie;        
        }

        // Retourne le contenu final de la réservation
        res = new Reservation(nomReservation, id, date, (string)horaire);
        return res;
    }

    // Initialisation de la structure Reservation
    struct Reservation
    {
        public string nom;
        public int idLiaison;
        public string date;
        public string heure;
        public string horodatage;
        
        // Constructeur
        public Reservation(string nm, int id, string dt, string h)
        {
            nom = nm;
            idLiaison = id;
            date = dt;
            heure = h;
            horodatage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

    }


    // Initialisation de la structure Passagers
    struct Passagers
    {
        public string nom;
        public string prenom;
        public string codeCategorie;

        // Constructeur
        public Passagers(string nm, string pn, string cd)
        {
            nom = nm;
            prenom = pn;
            codeCategorie = cd;
        }
    }
}