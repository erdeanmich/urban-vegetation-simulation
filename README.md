# Procedural Urban Vegetation Generation
## Disclaimer 
This project is work in progress!

## Description
This package helps you to bring a procedural ecosystem to your urban scenes. 
You've got the freedom to configure your own vegetation assets to fit to urban zones in your city.

## Features
* Creation and simulation of an ecosystem consisting of your own plants
* Maintenance of vegetation in different urban zones (residential, transportion, green areas, ...) in your game

## How to use
* Right click in your project structure > Create > ScriptableObjects
* Create a PlantPrototypePool and a UrbanZonePrototype pools
* Create plant prototypes! 
  * You can use the PlantScaffold prefab to create a variant, configure the prototype in the prefab
  * For each plant prototype you have to implement a PlantDrawable and PlantGrowable
  * Reference your prototype in the PlantPrototypePool
* Create urban zone prototypes!
  * You can use the UrbanZoneScaffold prefab to create a variant, configure the prototype in the prefab
  * For each zone prototype you have to implement and reference an UrbanZoneIdentifiable
  * Reference your prototype in the UrbanZonePrototypePool
* Put the UrbanVegetationSimulation prefab in your city scene 
    * Reference your prototype pools
    * Create an adapter class which calls the InitSimulation() method of the attached EcosystemSimulation component
    * Start the simulation with StartSimulation() or start individual steps SimulateStep()
