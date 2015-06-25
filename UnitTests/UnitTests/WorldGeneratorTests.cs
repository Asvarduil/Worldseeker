using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityEngine;

namespace UnitTests
{
    [TestClass]
    public class WorldGeneratorTests
    {
        #region Variables / Properties

        private Room _firstRoom;
        private Room _secondRoom;

        #endregion Variables / Properties

        #region Tests

        [TestMethod]
        public void AreOverlappingRoomsProperlyDetected()
        {
            GivenTwoOverlappingRoomsExist();
            bool areOverlapping = WhenIDetectIfTheRoomsAreOverlapping();
            ThenTheRoomsOverlap(areOverlapping);
        }

        [TestMethod]
        public void AreSeparateRoomsProperlyDetected()
        {
            GivenTwoDisjunctRoomsExist();
            bool areOverlapping = WhenIDetectIfTheRoomsAreOverlapping();
            ThenTheRoomsDoNotOverlap(areOverlapping);
        }

        [TestMethod]
        public void AreConnectedRoomsProperlyDetected()
        {
            GivenTwoAdjacentRoomsExist();
            bool areConnected = WhenIDetectIfTheRoomsAreAdjacent();
            ThenTheRoomsAreAdjacent(areConnected);
        }

        [TestMethod]
        public void AreDisconnectedRoomsProperlyDetected()
        {
            GivenTwoDisjunctRoomsExist();
            bool areConnected = WhenIDetectIfTheRoomsAreAdjacent();
            ThenTheRoomsAreNotAdjacent(areConnected);
        }

        [TestMethod]
        public void AreAdjacentNonFacingRoomsProperlyDetected()
        {
            GivenTwoAdjacentNonFacingRoomsExist();
            bool areConnected = WhenIDetectIfTheRoomsAreAdjacent();
            ThenTheRoomsAreNotAdjacent(areConnected);
        }

        #endregion Tests

        #region Setup

        // [1][!][2]
        private void GivenTwoOverlappingRoomsExist()
        {
            var firstRoomPosition = new Vector3();
            var firstCellPosition = new Vector3();
            var leftCellPosition = new Vector3
            {
                x = -1,
                y = 0,
                z = 0
            };
            var rightCellPosition = new Vector3
            {
                x = 1,
                y = 0,
                z = 0
            };

            var firstRoomCells = new List<RoomCell>
            {
                new RoomCell
                {
                    Position = firstCellPosition,
                    CellType = RoomCellType.Space
                },
                new RoomCell
                {
                    Position = rightCellPosition,
                    CellType = RoomCellType.Door
                }
            };

            var secondRoomPosition = new Vector3
            {
                x = 2,
                y = 0,
                z = 0
            };

            var secondRoomCells = new List<RoomCell>
            {
                new RoomCell
                {
                    Position = firstCellPosition,
                    CellType = RoomCellType.Space
                },
                new RoomCell
                {
                    Position = leftCellPosition,
                    CellType = RoomCellType.Door
                }
            };

            _firstRoom = new Room
            {
                Position = firstRoomPosition,
                Name = "First Room",
                Id = 0,
                Cells = firstRoomCells
            };

            _secondRoom = new Room
            {
                Position = secondRoomPosition,
                Name = "Second Room",
                Id = 1,
                Cells = secondRoomCells
            };
        }

        // [1][D->][<-D][2]
        private void GivenTwoAdjacentRoomsExist()
        {
            var firstRoomPosition = new Vector3();
            var firstCellPosition = new Vector3();
            var leftCellPosition = new Vector3
            {
                x = -1,
                y = 0,
                z = 0
            };
            var leftCellRotation = new Vector3
            {
                x = 0,
                y = 0,
                z = 0
            };
            var rightCellPosition = new Vector3
            {
                x = 1,
                y = 0,
                z = 0
            };
            var rightCellRotation = new Vector3
            {
                x = 0,
                y = 180,
                z = 0
            };

            var firstRoomCells = new List<RoomCell>
            {
                new RoomCell
                {
                    Position = firstCellPosition,
                    CellType = RoomCellType.Space
                },
                new RoomCell
                {
                    Position = rightCellPosition,
                    Rotation = rightCellRotation,
                    CellType = RoomCellType.Door
                }
            };

            var secondRoomPosition = new Vector3
            {
                x = 3,
                y = 0,
                z = 0
            };

            var secondRoomCells = new List<RoomCell>
            {
                new RoomCell
                {
                    Position = firstCellPosition,
                    CellType = RoomCellType.Space
                },
                new RoomCell
                {
                    Position = leftCellPosition,
                    Rotation = leftCellRotation,
                    CellType = RoomCellType.Door
                }
            };

            _firstRoom = new Room
            {
                Position = firstRoomPosition,
                Name = "First Room",
                Id = 0,
                Cells = firstRoomCells
            };

            _secondRoom = new Room
            {
                Position = secondRoomPosition,
                Name = "Second Room",
                Id = 1,
                Cells = secondRoomCells
            };
        }

        // [1][<-D][D->][2]
        private void GivenTwoAdjacentNonFacingRoomsExist()
        {
            var firstRoomPosition = new Vector3();
            var firstCellPosition = new Vector3();
            var leftCellPosition = new Vector3
            {
                x = -1,
                y = 0,
                z = 0
            };
            var leftCellRotation = new Vector3
            {
                x = 0,
                y = 0,
                z = 0
            };
            var rightCellPosition = new Vector3
            {
                x = 1,
                y = 0,
                z = 0
            };
            var rightCellRotation = new Vector3
            {
                x = 0,
                y = 180,
                z = 0
            };

            var firstRoomCells = new List<RoomCell>
            {
                new RoomCell
                {
                    Position = firstCellPosition,
                    CellType = RoomCellType.Space
                },
                new RoomCell
                {
                    Position = rightCellPosition,
                    Rotation = leftCellRotation,
                    CellType = RoomCellType.Door
                }
            };

            var secondRoomPosition = new Vector3
            {
                x = 3,
                y = 0,
                z = 0
            };

            var secondRoomCells = new List<RoomCell>
            {
                new RoomCell
                {
                    Position = firstCellPosition,
                    CellType = RoomCellType.Space
                },
                new RoomCell
                {
                    Position = leftCellPosition,
                    Rotation = rightCellRotation,
                    CellType = RoomCellType.Door
                }
            };

            _firstRoom = new Room
            {
                Position = firstRoomPosition,
                Name = "First Room",
                Id = 0,
                Cells = firstRoomCells
            };

            _secondRoom = new Room
            {
                Position = secondRoomPosition,
                Name = "Second Room",
                Id = 1,
                Cells = secondRoomCells
            };
        }

        // [D][1] *  * [2][D]
        private void GivenTwoDisjunctRoomsExist()
        {
            var firstRoomPosition = new Vector3();
            var firstCellPosition = new Vector3();
            var leftCellPosition = new Vector3
            {
                x = -1,
                y = 0,
                z = 0
            };
            var rightCellPosition = new Vector3
            {
                x = 1,
                y = 0,
                z = 0
            };

            var firstRoomCells = new List<RoomCell>
            {
                new RoomCell
                {
                    Position = firstCellPosition,
                    CellType = RoomCellType.Space
                },
                new RoomCell
                {
                    Position = rightCellPosition,
                    CellType = RoomCellType.Door
                }
            };

            var secondRoomPosition = new Vector3
            {
                x = 3,
                y = 0,
                z = 0
            };

            var secondRoomCells = new List<RoomCell>
            {
                new RoomCell
                {
                    Position = firstCellPosition,
                    CellType = RoomCellType.Space
                },
                new RoomCell
                {
                    Position = leftCellPosition,
                    CellType = RoomCellType.Door
                }
            };

            _firstRoom = new Room
            {
                Position = secondRoomPosition,
                Name = "First Room",
                Id = 0,
                Cells = firstRoomCells
            };

            _secondRoom = new Room
            {
                Position = firstRoomPosition,
                Name = "Second Room",
                Id = 1,
                Cells = secondRoomCells
            };
        }

        #endregion Setup

        #region Actuation

        private bool WhenIDetectIfTheRoomsAreAdjacent()
        {
            return Room.AreRoomDoorsJoined(_firstRoom, _secondRoom);
        }

        private bool WhenIDetectIfTheRoomsAreOverlapping()
        {
            return Room.DoRoomsOverlap(_firstRoom, _secondRoom);
        }

        #endregion Actuation

        #region Validation

        private void ThenTheRoomsOverlap(bool areOverlapping)
        {
            Assert.IsTrue(areOverlapping, "The two rooms that overlap were somehow detected as disjunct from each other.");
        }

        private void ThenTheRoomsDoNotOverlap(bool areOverlapping)
        {
            Assert.IsFalse(areOverlapping, "The two rooms that are set up airgapped were somehow detected as being overlapping.");
        }

        private void ThenTheRoomsAreAdjacent(bool areAdjacent)
        {
            Assert.IsTrue(areAdjacent, "The two rooms that are set up with adjacent doors don't register as being connected by their doors.");
        }

        private void ThenTheRoomsAreNotAdjacent(bool areAdjacent)
        {
            Assert.IsFalse(areAdjacent, "The two rooms that are set up with their doors not adjacent were somehow detected as being adjacent.");
        }

        #endregion Validation
    }
}
