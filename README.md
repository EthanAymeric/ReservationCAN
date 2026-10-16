# 🚢 - Compagnie Alsacienne de Navigation (CAN)

Projet mêlant création et enregistrement dans un fichier JSON d'une réservation par un terminal fonctionnant par un code en C# et récupération et affichage sur un site web des informations de toutes les réservations.

Ce projet consiste à développer un système informatique complet pour la **CAN (Compagnie Alsacienne de Navigation)** afin de gérer la réservation et l'après-vente de traversées maritimes sur 4 liaisons (Lorient - Groix & Quiberon - Belle-Île) pour le mois de **Novembre 2025**.

---

## 📐 Architecture du Projet

Le système repose sur deux composants applicatifs interconnectés par un flux d'échange au format **JSON** :

```
 ┌────────────────┐                ┌─────────────┐                ┌────────────────┐
 │ Application C# │ ──(Génère)───► │  Fichier    │ ──(Dépôt)────► │    Serveur     │
 │ (Mode Console) │                │    JSON     │                │  Central/API   │
 └────────────────┘                └─────────────┘                └────────────────┘
                                                                          │
                                                                   (Flux JSON)
                                                                          ▼
                                                                  ┌────────────────┐
                                                                  │ Application Web│
                                                                  │ (HTML/CSS/JS)  │
                                                                  └────────────────┘
```

1. **Partie C# (P11 - Console)** : Application de prise de réservation côté guichet / client.
2. **Partie Web (W11 - HTML/CSS/JS)** : Site après-vente et espace de suivi pour les usagers et le personnel de la CAN.

---

## 🛠️ Partie P11 : Logiciel de Réservation (C# Console)

L'application console C# permet d'effectuer des réservations guidées étape par étape.

### Fonctionnalités
- **Étape 1 : Choix de la traversée**
  - Saisie du nom du responsable de la réservation.
  - Sélection parmi les 4 liaisons gérées :
    1. *Lorient ➔ Groix* (id 1)
    2. *Groix ➔ Lorient* (id 2)
    3. *Quiberon ➔ Le Palais* (id 3)
    4. *Le Palais ➔ Quiberon* (id 4)
  - Sélection de la date (Novembre 2025) et de l'horaire de départ en fonction des grilles fournies.
- **Étape 2 : Saisie des Passagers & Véhicules**
  - Ajout des passagers : nom, prénom, catégorie (`adu26p`, `jeu1825`, `enf417`, `bebe`, `ancomp`).
  - Ajout des véhicules : catégorie (`trot`, `velo`, `cat1`, `cat2`, `cat3`, `cat4`, `camp`, etc.) et quantité.
- **Étape 3 : Récapitulatif et Calcul du Tarif**
  - Calcul automatique du tarif global en fonction des grilles tarifaires de chaque ligne.
  - Affichage d'un bilan récapitulatif avant confirmation.
- **Étape 4 : Exportation JSON**
  - Génération d'un fichier JSON structuré, prêt à être transmis au serveur central pour enregistrement.

---

## 🌐 Partie W11 : Site Web Après-Vente (HTML / CSS / JS)

Le site web interactif exploite les flux JSON pour restituer les informations et afficher des tableaux de bord.

### Pages & Fonctionnalités
- **🏠 Page d'accueil** : Menu principal de navigation ergonomique.
- **🎟️ Cartes d'embarquement** :
  - Saisie du numéro de réservation.
  - Affichage individuel de chaque carte (passager ou véhicule) avec navigation (flèches).
  - Design adaptatif (Responsive Design / Mobile First) conçu pour affichage et scan sur smartphone à l'embarquement.
- **📄 Page Facture** :
  - Génération de la facture détaillée avec calculs de sous-totaux (passagers, véhicules) et total TTC.
- **📊 Tableau de bord (Ligne & Date)** :
  - Visualisation des taux de remplissage des traversées par date (Passagers & Véhicules cat. 1 à 4).
  - Code couleur dynamique sur le taux de remplissage des véhicules :
    - 🟢 **Vert** : $< 50\%$
    - 🟠 **Orange** : $50\%$ à $75\%$
    - 🔴 **Rouge** : $75\%$ à $100\%$
    - 🟡 **Jaune fluo** : $> 100\%$ (surréservation)
- **📈 Page Statistiques** :
  - Chiffres d'affaires globaux et détaillés (passagers vs véhicules) pour le mois de novembre 2025.
  - Volumétrie des véhicules et passagers transportés par liaison.

---

## 🎨 Contraintes Techniques & Orientations

- **Format d'échange :** JSON (conforme au schéma exigé dans le sujet).
- **Mise en page Web :** Utilisation exclusive de **Flexbox** pour le positionnement CSS.
- **JavaScript :** Manipulation du DOM et parsing asynchrone des flux JSON sans bibliothèques tierces superflues.
- **Code :** Respect des conventions de nommage et code abondamment commenté.

---

## 👥 Contributeurs

- [alexiaa26](https://github.com/alexiaa26)
- [EthanAymeric](https://github.com/EthanAymeric)
- [GalatiMaxime](https://github.com/GalatiMaxime)
