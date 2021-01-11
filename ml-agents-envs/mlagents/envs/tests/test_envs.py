import unittest.mock as mock
import pytest

import numpy as np

from mlagents.envs.environment import UnityEnvironment
from mlagents.envs.exception import UnityEnvironmentException, UnityActionException
from mlagents.envs.brain import BrainInfo
from mlagents.envs.mock_communicator import MockCommunicator


@mock.patch("mlagents.envs.environment.UnityEnvironment.get_communicator")
def test_handles_bad_filename(get_communicator):
    with pytest.raises(UnityEnvironmentException):
        UnityEnvironment(" ")


@mock.patch("mlagents.envs.environment.UnityEnvironment.executable_launcher")
@mock.patch("mlagents.envs.environment.UnityEnvironment.get_communicator")
def test_initialization(mock_communicator, mock_launcher):
    mock_communicator.return_value = MockCommunicator(
        discrete_action=False, visual_inputs=0
    )
    env = UnityEnvironment(" ")
    assert env.external_brain_names[0] == "RealFakeBrain"
    env.close()


@mock.patch("mlagents.envs.environment.UnityEnvironment.executable_launcher")
@mock.patch("mlagents.envs.environment.UnityEnvironment.get_communicator")
def test_reset(mock_communicator, mock_launcher):
    mock_communicator.return_value = MockCommunicator(
        discrete_action=False, visual_inputs=0
    )
    env = UnityEnvironment(" ")
    brain = env.brains["RealFakeBrain"]
    brain_info = env.reset()
    env.close()
    assert isinstance(brain_info, dict)
    assert isinstance(brain_info["RealFakeBrain"], BrainInfo)
    assert isinstance(brain_info["RealFakeBrain"].visual_observations, list)
    assert isinstance(brain_info["RealFakeBrain"].vector_observations, np.ndarray)
    assert (
        len(brain_info["RealFakeBrain"].visual_observations)
        == brain.number_visual_observations
    )
    assert len(brain_info["RealFakeBrain"].vector_observations) == len(
        brain_info["RealFakeBrain"].agents
    )
    assert (
        len(brain_info["RealFakeBrain"].vector_observations[0])
        == brain.vector_observation_space_size
    )


@mock.patch("mlagents.envs.environment.UnityEnvironment.executable_launcher")
@mock.patch("mlagents.envs.environment.UnityEnvironment.get_communicator")
def test_step(mock_communicator, mock_launcher):
    mock_communicator.return_value = MockCommunicator(
        discrete_action=False, visual_inputs=0
    )
    env = UnityEnvironment(" ")
    brain = env.brains["RealFakeBrain"]
    brain_info = env.step()
    brain_info = env.step(
        [0]
        * brain.vector_action_space_size[0]
        * len(brain_info["RealFakeBrain"].agents)
    )
    with pytest.raises(UnityActionException):
        env.step([0])
    brain_info = env.step(
        [-1]
        * brain.vector_action_space_size[0]
        * len(brain_info["RealFakeBrain"].agents)
    )
    env.close()
    assert isinstance(brain_info, dict)
    assert isinstance(brain_info["RealFakeBrain"], BrainInfo)
    assert isinstance(brain_info["RealFakeBrain"].visual_observations, list)
    assert isinstance(brain_info["RealFakeBrain"].vector_observations, np.ndarray)
    assert (
        len(brain_info["RealFakeBrain"].visual_observations)
        == brain.number_visual_observations
    )
    assert len(brain_info["RealFakeBrain"].vector_observations) == len(
        brain_info["RealFakeBrain"].agents
    )
    assert (
        len(brain_info["RealFakeBrain"].vector_observations[0])
        == brain.vector_observation_space_size
    )

    print("\n\n\n\n\n\n\n" + str(brain_info["RealFakeBrain"].local_done))
    assert not brain_info["RealFakeBrain"].local_done[0]
    assert brain_info["RealFakeBrain"].local_done[2]


@mock.patch("mlagents.envs.environment.UnityEnvironment.executable_launcher")
@mock.patch("mlagents.envs.environment.UnityEnvironment.get_communicator")
def test_close(mock_communicator, mock_launcher):
    comm = MockCommunicator(discrete_action=False, visual_inputs=0)
    mock_communicator.return_value = comm
    env = UnityEnvironment(" ")
    assert env._loaded
    env.close()
    assert not env._loaded
    assert comm.has_been_closed


def test_returncode_to_signal_name():
    assert UnityEnvironment.returncode_to_signal_name(-2) == "SIGINT"
    assert UnityEnvironment.returncode_to_signal_name(42) is None
    assert UnityEnvironment.returncode_to_signal_name("SIGINT") is None


if __name__ == "__main__":
    pytest.main()
