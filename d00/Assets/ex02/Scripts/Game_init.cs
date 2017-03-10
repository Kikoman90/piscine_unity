using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_init : MonoBehaviour {

	public Player	player_prefab;

	public ParticleSystem	Score_particles;

	public Transform[]	spawns = new Transform[3];
	public Transform[]	holes = new Transform[3];
	public Transform[]	limits = new Transform[2];

	private ParticleSystem[]	particles = new ParticleSystem[3];

	private Player	_bot = null;
	private Player	_playerAI = null;
	private Player	_playerSP = null;

	private List<Player>	_players = null;

	private bool	_modeIsAI = false;
	private bool	_modeIsSP = false;
	private bool	_modeIsMP = false;
	private bool	_powerInc = true; /* to clamp the power between 0 and 1 */
	private bool	_hasToAppear = true; /* for mp, pretty self-explanatory huh */
	private bool	_hasToDisappear = false; /* for mp, pretty self-explanatory huh */
	private bool	_AIturn = true;
	private bool	_AIappear = true; /* because of the Input.GetKey */

	private int	_cur_idx = 0;

	private	int[]	_highscores = new int[]{ 1000, 1000, 1000 };

	private uint	_AIDifficulty = 1; /* default value */
	private uint	_nbOfPlayers = 2; /* default value */

	private float	_power = 0.0f;

	void			Start() {
		Vector3 ps_pos_0 = new Vector3 (holes [0].position.x, holes [0].position.y, holes [0].position.z - 5.0f);
		Vector3 ps_pos_1 = new Vector3 (holes [1].position.x, holes [1].position.y, holes [1].position.z - 5.0f);
		Vector3 ps_pos_2 = new Vector3 (holes [2].position.x, holes [2].position.y, holes [2].position.z - 5.0f);
		particles[0] = Instantiate (Score_particles, ps_pos_0, Quaternion.identity);
		particles[1] = Instantiate (Score_particles, ps_pos_1, Quaternion.identity);
		particles[2] = Instantiate (Score_particles, ps_pos_2, Quaternion.identity);
	}

	public void		Launch_AI() {
		if (_modeIsAI == false) { /* prevents from multiple button clicks resetting the gamemode */
			_modeIsAI = true;
			_modeIsSP = false;
			_modeIsMP = false;
			if (_bot == null && _playerAI == null) {
				_bot = Instantiate (player_prefab, spawns [0].position, Quaternion.identity);
				_playerAI = Instantiate (player_prefab, spawns [0].position, Quaternion.identity);
				_bot.InitPlayer ("bot", holes[0], limits[0], limits[1]);
				_playerAI.InitPlayer ("player", holes[0], limits[0], limits[1]);
				_playerAI.switchRender ();
			}
			if (_playerSP != null)
				_playerSP.setPlayerInactive ();
			if (_players != null) {
				foreach (Player pl in _players)
					pl.setPlayerInactive ();
			}
			if (_bot)
				_bot.setPlayerActive ();
			if (_playerAI)
				_playerAI.setPlayerActive ();
		}
	}

	public void		Launch_SP() {
		if (_modeIsSP == false) { /* prevents from multiple button clicks resetting the gamemode */
			_modeIsAI = false;
			_modeIsSP = true;
			_modeIsMP = false;
			if (_playerSP == null) {
				_playerSP = Instantiate (player_prefab, spawns [1].position, Quaternion.identity);
				_playerSP.InitPlayer ("player", holes[1], limits[0], limits[1]);
				_playerSP.switchRender ();
			}
			if (_bot != null)
				_bot.setPlayerInactive ();
			if (_playerAI != null)
				_playerAI.setPlayerInactive ();
			if (_players != null) {
				foreach (Player pl in _players)
					pl.setPlayerInactive ();
			}
			_playerSP.setPlayerActive ();
		}
	}

	public void		Launch_MP() {
		if (_modeIsMP == false) { /* prevents from multiple button clicks resetting the gamemode */
			_modeIsAI = false;
			_modeIsSP = false;
			_modeIsMP = true;
			if (_players == null) {
				_players = new List<Player>();
				for (int i = 0; i < _nbOfPlayers; i++) {
					Player	pl = Instantiate (player_prefab, spawns [2].position, Quaternion.identity);
					pl.InitPlayer ("player " + (i + 1), holes[2], limits[0], limits[1]);
					_players.Add (pl);
				}
			}
			if (_playerSP != null)
				_playerSP.setPlayerInactive ();
			if (_bot != null)
				_bot.setPlayerInactive ();
			if (_playerAI != null)
				_playerAI.setPlayerInactive ();
			foreach (Player pl in _players)
				pl.setPlayerActive ();
		}
	}

	public void		Reset_mode() {
		_power = 0.0f;
		if (_modeIsAI) {
			if (_bot != null)
				_bot.Die ();
			if (_playerAI != null)
				_playerAI.Die ();
			_hideWinner ("Canvas/ai_winner");
			_AIturn = true;
			_AIappear = true;
			_modeIsAI = false;
			Launch_AI ();
		} else if (_modeIsSP) {
			_playerSP.Die ();
			_modeIsSP = false;
			Launch_SP ();
		} else if (_modeIsMP) {
			foreach (Player pl in _players)
				pl.Die ();
			_players.Clear ();
			_players = null;
			_cur_idx = 0;
			_hideWinner ("Canvas/mp_winner");
			_hasToAppear = true;
			_hasToDisappear = false;
			_modeIsMP = false;
			Launch_MP ();
		}
	}

	public void		decBotDifficulty() {
		if (_AIDifficulty > 1) {
			_AIDifficulty--;
			transform.Find ("Canvas/BotDifficulty/Text").gameObject.GetComponent<Text> ().text = _AIDifficulty.ToString();
			Reset_mode ();
		}
	}

	public void		incBotDifficulty() {
		if (_AIDifficulty < 3) {
			_AIDifficulty++;
			transform.Find ("Canvas/BotDifficulty/Text").gameObject.GetComponent<Text> ().text = _AIDifficulty.ToString();
			Reset_mode ();
		}
	}

	public void		decNbOfPlayers() {
		if (_nbOfPlayers > 2) {
			_nbOfPlayers--;
			transform.Find ("Canvas/NbOfPlayers/Text").gameObject.GetComponent<Text> ().text = _nbOfPlayers.ToString();
			Reset_mode ();
		}
	}

	public void		incNbOfPlayers() {
		if (_nbOfPlayers < 9) {
			_nbOfPlayers++;
			transform.Find ("Canvas/NbOfPlayers/Text").gameObject.GetComponent<Text> ().text = _nbOfPlayers.ToString();
			Reset_mode ();
		}
	}

	private void	_hideWinner(string path) {
		GameObject	winner;

		winner = transform.Find (path).gameObject;
		winner.GetComponentInChildren<Text> ().text = string.Empty;
		winner.GetComponent<moving_button> ().permanent = false;
		winner.GetComponent<moving_button> ().HideButton ();
	}

	private void	_displayWinner(string path, string msg) {
		GameObject	winner;

		winner = transform.Find (path).gameObject;
		winner.GetComponentInChildren<Text> ().text = msg;
		winner.GetComponent<moving_button> ().permanent = true;
		winner.GetComponent<moving_button> ().ShowButton ();
	}

	/* nb of frames = dist * 27.0f / 10.0f
	* after testing, the ball needed 27 frames (1 frame ~= 0.016s) to reach the hole
	* at a distance of 10.0f (27frames gave smooth movement): this gives us the optimal
	* number of frames needed for the ball to reach the hole proportionnal to the distance
	* deceleration being of 0.02f by frame, and assuming the final speed is 0.0f, we use
	* the formula of deceleration: D = (vf - vi) / t with {D = deceleration, vf = final speed,
	* vi = initial speed, t = time (how many frames it takes to get from vi to vf)}
	* which give us: vi = vf - (D * t) = 0.0f - (-0.02f * (dist_ball-hole * (27.0f / 10.0f)))
	* we then divide the result by the constant value we chose for the ball.transform.translate: 50.0f * Time.deltaTime
	* we get: _power = vi / (50.0f  * Time.deltaTime) = vi / (50.0f * 0.016)
	* just in case: Mathf.Clamp(_power, 0.0f, 1.0f); */
	private float	_getBotPower() {
		float	perfect_power;

		if (_AIDifficulty == 1)
			perfect_power = Random.Range (0.0f, 1.0f);
		else {
			perfect_power = Mathf.Clamp ((0.02f * Mathf.Abs (holes [0].position.y - _bot.getBallYpos ()) * (27.0f / 10.0f)) / (50.0f * 0.016f), 0.0f, 1.0f);
			if (_AIDifficulty == 2)
				perfect_power = Random.Range (Mathf.Clamp(perfect_power - 0.4f, 0.0f, 1.0f), Mathf.Clamp(perfect_power + 0.4f, 0.0f, 1.0f));
			else
				perfect_power = Random.Range (Mathf.Clamp(perfect_power - 0.2f, 0.0f, 1.0f), Mathf.Clamp(perfect_power + 0.2f, 0.0f, 1.0f));
		}
		return (perfect_power);
	}

	private void	_ai_update() {
		if (_playerAI != null && _playerAI.ballInHole ()) {
			particles [0].Play ();
			int score;
			if ((score = _playerAI.getScore ()) < _highscores [0]) {
				_highscores [0] = score;
				transform.Find ("Canvas/highscore_ai/Text").gameObject.GetComponent<Text> ().text = "highscore: " + _highscores [0];
			}
			if (_bot != null) {
				_displayWinner ("Canvas/ai_winner", "winner: player");
				_playerAI.Die ();
				_AIappear = true;
				_AIturn = true;
			} else
				Reset_mode ();
		}
		if (_bot != null && _bot.ballInHole ()) {
			particles [0].Play ();
			int	score;
			if ((score = _bot.getScore ()) < _highscores [0]) {
				_highscores [0] = score;
				transform.Find ("Canvas/highscore_ai/Text").gameObject.GetComponent<Text> ().text = "highscore: " + _highscores [0];
			}
			if (_playerAI != null) {
				_displayWinner ("Canvas/ai_winner", "winner: bot");
				_bot.Die ();
				_AIappear = false;
				_AIturn = false;
			} else
				Reset_mode ();
		}
		if (_AIturn == false && _playerAI.ballMovement () == false && (_bot == null || (_bot != null && _bot.ballMovement() == false))) {
			if (_AIappear == false) {
				_playerAI.switchRender ();
				if (_bot != null)
					_bot.switchRender ();
				_AIappear = true;
			}
			if (_AIturn == false) {
				if (Input.GetKey ("space")) {
					if (_power >= 1.0f)
						_powerInc = false;
					else if (_power <= 0.0f)
						_powerInc = true;
					_power += (_powerInc == true) ? 0.032f : -0.032f;
					_playerAI.moveClub (_powerInc);
				} else if (_power > 0.0f) {
					_playerAI.shoot (_power);
					if (_bot != null)
						_AIturn = true;
					_power = 0.0f;
				}
			}
		}
		else if (_AIturn && _bot.ballMovement () == false && ((_playerAI == null) || (_playerAI != null && _playerAI.ballMovement () == false))) {
			if (_AIappear) {
				_bot.switchRender ();
				if (_playerAI != null)
					_playerAI.switchRender ();
				_AIappear = false;
			}
			if (_AIturn) {
				_bot.shoot (_getBotPower ());
				if (_playerAI != null)
					_AIturn = false;
			}
		}
	}

	private void	_sp_update() {
		if (_playerSP.ballMovement () == false) {
			if (_playerSP.ballInHole ()) {
				particles[1].Play();
				int	score;
				if ((score = _playerSP.getScore ()) < _highscores [1]) {
					if (score == -10)
						gameObject.GetComponentInChildren<bonus_bunny> ().Activate ();
					_highscores [1] = score;
					transform.Find ("Canvas/highscore_sp/Text").gameObject.GetComponent<Text> ().text = "highscore: " + _highscores [1];
				}
				Reset_mode ();
			}
			if (Input.GetKey ("space")) {
				if (_power >= 1.0f)
					_powerInc = false;
				else if (_power <= 0.0f)
					_powerInc = true;
				_power += (_powerInc == true) ? 0.032f : -0.032f;
				_playerSP.moveClub (_powerInc);
			} else if (_power > 0.0f) {
				_playerSP.shoot (_power);
				Debug.Log ("Score: " + _playerSP.getScore()); /* asked by the subject smh */
				_power = 0.0f;
			}
		}
	}

	private void	_mp_update() {
		int prev_idx = 0;
		if (_players != null)
			prev_idx = (_cur_idx == 0) ? _players.Count - 1 : _cur_idx - 1;
		if (_players [_cur_idx].ballMovement () == false && _players [prev_idx].ballMovement () == false) {
			if (_players [prev_idx].ballInHole()) {
				particles [2].Play ();
				int	score;
				if ((score = _players[prev_idx].getScore ()) < _highscores [2]) {
					_highscores [2] = score;
					transform.Find ("Canvas/highscore_mp/Text").gameObject.GetComponent<Text> ().text = "highscore: " + _highscores [2];
				}
				if (_players.Count == _nbOfPlayers)
					_displayWinner ("Canvas/mp_winner", "winner: " + _players [prev_idx].getName ());
				if (_players.Count == 1)
					Reset_mode ();
				else {
					_players [prev_idx].Die ();
					_players.Remove (_players [prev_idx]);
					_hasToDisappear = false;
					if (_cur_idx != 0)
						_cur_idx = prev_idx;
				}
			}
			if (_hasToAppear) {
				_players [_cur_idx].switchRender ();
				_hasToAppear = false;
			}
			if (_hasToDisappear) {
				_players [prev_idx].switchRender ();
				_hasToDisappear = false;
			}
			if (Input.GetKey ("space")) {
				if (_power >= 1.0f)
					_powerInc = false;
				else if (_power <= 0.0f)
					_powerInc = true;
				_power += (_powerInc == true) ? 0.032f : -0.032f;
				_players[_cur_idx].moveClub (_powerInc);
			} else if (_power > 0.0f) {
				_players[_cur_idx].shoot (_power);
				if (_players.Count > 1) {
					_hasToAppear = true;
					_hasToDisappear = true;
					_cur_idx = (_cur_idx < _players.Count - 1) ? _cur_idx + 1 : 0;
				}
				_power = 0.0f;
			}
		}
	}

	void			Update () {
		if (_modeIsAI == true)
			_ai_update ();
		else if (_modeIsSP == true)
			_sp_update ();
		else if (_modeIsMP == true)
			_mp_update ();
	}
}
