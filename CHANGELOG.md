# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.0.9] - 2023-08-01
### Added
* `CanRepeatNodesInPath` to `IPathfinding` to allow for repeating nodes or retracing nodes when calling `TryAddNodeToPath`
### Fixed
* bug where `TryAddNodeToPath` would call `GetMoveCostBetweenNodes` without properly initializing parameter nodes

## [0.0.8] - 2023-06-16
### Changed
* `NodePath`'s fields to be initialized when the constructor is called rather than through serialization

## [0.0.7] - 2023-04-19
### Added
* `SetNeighbor(int index, Node neighbor)` and `GetNeighbor(int)` and `HasNeighbor(int)`
### Changed
* `Node` property names
* `Node` neighbors to be ordered by using a sparsely packed list
* `Node`.`AddNeighbor` to handle sparsely packed list data structure
### Fixed
* `AStarPathfinding` bug where `IsValidMoveBetweenNodes()` evaluated incorrect nodes

## [0.0.6] - 2023-04-12
### Changed
* pooling API calls to Kokowolo.Utilities after updating Utilities package version

## [0.0.5] - 2023-03-23
### Added
* `GetAllSearchedNodes` and `GetPreexistingPath` to `AStarPathfinding` which allow for grabbing all navigable nodes
* `GetDistanceBetweenNodes` to `IPathfinding` to properly query for the search's heuristic
### Changed
* `AStarPathfinding`.`Search` to `GetPath`
### Fixed
* incorrect search heuristic of 0 back to a correct value utilizing `GetDistanceBetweenNodes`

## [0.0.4] - 2023-03-22
### Changed
* `NodePath`'s `Copy(List<Node>)` and `Add(Node, int distance)` to `internal` since its functionality isn't obvious
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