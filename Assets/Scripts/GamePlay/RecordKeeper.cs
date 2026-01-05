using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class RecordKeeper : Singleton<RecordKeeper> {
	[System.Serializable]
	public class Data {
		[System.Serializable]
		public class Record {
			[SerializeField] string name;
			[SerializeField] int val;

			public Record (string name, int value){
				this.name = name;
				this.val = value;
			}

			public string Name {
				get {
					return this.name;
				}
				set {
					name = value;
				}
			}

			public int Val {
				get {
					return this.val;
				}
				set {
					val = value;
				}
			}
		}
		[SerializeField] List<Record> records;

		public List<Record> Records {
			get {
				return this.records;
			}
			set {
				records = value;
			}
		}

		public static int SortByValue(Record r1, Record r2) {
			return r2.Val.CompareTo (r1.Val);
		}
	}

	[SerializeField] Data data;
	[SerializeField] bool hasBeenLoaded;

	public Data RecordData {
		get {
			return this.data;
		}
		set {
			data = value;
		}
	}

	public void AddNewRecord(string name, int value) {
		data.Records.Add (new Data.Record (name, value));
		data.Records.Sort (Data.SortByValue);
	}

	public void Save() {
		string filename = Application.persistentDataPath + "/data.dat";
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (filename, FileMode.OpenOrCreate);
		bf.Serialize (file, data);
		file.Close ();
	}

	public void Load() {
		string filename = Application.persistentDataPath + "/data.dat";
		if (File.Exists (filename)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (filename, FileMode.Open);
			data = (Data)bf.Deserialize (file);
			file.Close ();
			hasBeenLoaded = true;
		} 
		else {
			Save ();
		}
	}
		
	void Awake() {
		DontDestroyOnLoad (this);

		if (FindObjectsOfType<RecordKeeper> ().Length > 1) {
			Destroy (gameObject);
			return;
		}

		if (!hasBeenLoaded) {
			Load ();
		}

		InitiateSingleton ();
	}
}
