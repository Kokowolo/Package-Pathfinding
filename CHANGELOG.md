# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.0.4] - 2023-03-22
### Changed
* `NodePath`.`Copy(List<Node>)` to `internal` since its functionality isn't obvious
### Fixed
* `NodePath`'s incorrect distance calculation

## [0.0.3] - 2023-03-20
### Fixed
* `Node`.`ToString` bug where `Node` was simply returning itself as a string

## [0.0.2] - 2023-03-02
### Added
* `Node`.`ClearNeighbors` such that a node's neighbors can now be reset
### Changed 
* `AStarPathfinding`.`searchFrontierPhase` to be a public get, private set property
* `NodeDebugObject`'s initialization and refresh functionality, simplifying the implementation

## [0.0.1] - 2023-02-28
### Added
* initial commit of package (extacted from current project)
* `NodeDebugObject` class and prefab (previously `GridCellDebugObject`, which now has been refactored out from `Kokowolo`.`Grid`)
* `IsWalkable` to `Node`