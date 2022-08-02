using UnityEngine;
using Zenject;
using Dices.UIConnection;
using Doozy.Engine.Soundy;

namespace Dices.Donation
    {
    public class DonationShowStart : MonoBehaviour // Class for control show after succerful donation
    {
        [Inject]
        DonationManager _donationManager;

        [SerializeField]
        private GameObject _showDonationCamera, _showDonationFlower, _trail_L, _trail_R;
        [SerializeField]
        private ParticleSystem[] _bigFlower;
        private int[,] _indexationVariants = { { 0, 1, 2 }, { 0, 2, 1 }, { 1, 2, 0 }, { 1, 0, 2 }, { 2, 0, 1 }, { 2, 1, 0 } };
        private float[,] _colorSubtrahends = { {0, 62, 173}, {0, 5, 255}, {0, 143, 176}, { 0, 120, 156}, {0, 49, 156}, {0, 116, 191}, {0, 116, 191}, {0, 90, 173}, {0, 80, 131}, {0, 128, 197}, {0, 62, 173}, {0, 80, 182}};
        private float[] _alphaChanal = { 0, 0, 0, 0, 61, 127, 159, 0, 0, 0, 0, 128};
        [SerializeField]
        private float[] _currentColorSubtrahends, _colorSubtrahendsIndexated;
        [SerializeField]
        private float r, g, b;
        public Color _color;
        [SerializeField]
        private int _currentIndexationVariantIndex;
        [SerializeField]
        private int[] _currenIndexationVariant = new int[3];
        [SerializeField]
        private float[] _colorArray = new float[3];
        [SerializeField]
        private SoundyData _soundData;


        void CreateRandomFlower()
    {
            CreateRandomColor();
            GetIndexationVariant(_colorArray);

    }

        void CreateRandomColor()
        {
            r = Random.Range(0, 255);
            g = Random.Range(0, 255);
            b = Random.Range(0, 255);
            _colorArray[0] = r/255;
            _colorArray[1] = g/255;
            _colorArray[2] = b/255;
            _currenIndexationVariant = new int[3] { _indexationVariants[_currentIndexationVariantIndex, 0], _indexationVariants[_currentIndexationVariantIndex, 1], _indexationVariants[_currentIndexationVariantIndex, 2] };
            _color = new Color(_colorArray[0], _colorArray[1], _colorArray[2], 1f);

            int i = 0;
            foreach (ParticleSystem _flowerPart in _bigFlower)
            {
                _currentColorSubtrahends = new float[3] { _colorSubtrahends[i, 0], _colorSubtrahends[i, 1], _colorSubtrahends[i, 2] };
                CreateFlowerParts(_currenIndexationVariant, _colorArray, _flowerPart, _currentColorSubtrahends, i);
                i++;
            }
        }

        void CreateFlowerParts(int[] _indexationVariant, float[] _randomColorFloats, ParticleSystem _particleSystem, float[] _colorSubtrahend, int _partSysIndex)
        {

            _colorSubtrahendsIndexated = new float[3] {_colorSubtrahend[_indexationVariant[2]]/255, _colorSubtrahend[_indexationVariant[1]]/255, _colorSubtrahend[_indexationVariant[0]]/255, };

            int j = 0;
            foreach(float _colorFloat in _randomColorFloats)
            {
                _randomColorFloats[j] = Mathf.Abs(_randomColorFloats[j] - _colorSubtrahendsIndexated[j]);
                j++;
            }
        Color _color = new Color(_randomColorFloats[0], _randomColorFloats[1], _randomColorFloats[2], (255 - _alphaChanal[_partSysIndex])/255);
            _particleSystem.startColor = _color;
        }



        void GetIndexationVariant(float[] _randomColors)
        {
            if (_randomColors[0] >= _randomColors[1])
            {
                if (_randomColors[1] >= _randomColors[2])
                    _currentIndexationVariantIndex = 0;
                else
            if (_randomColors[0] >= _randomColors[2])
                    _currentIndexationVariantIndex = 1;
                else
                    _currentIndexationVariantIndex = 2;
            }
            else
            { if (_randomColors[0] >= _randomColors[2])
                {
                    _currentIndexationVariantIndex = 3;
                }
                else
                {
                    if (_randomColors[1] >= _randomColors[2])
                        _currentIndexationVariantIndex = 4;
                    else
                        _currentIndexationVariantIndex = 5;
                }
            }
        }

        void SetDefoultColors()
        {
            Color _o = new Color(255 / 255f, 82 / 255f, 193 / 255f);
            _bigFlower[0].startColor = _o;

            Color _a = new Color(220 / 255f, 0 / 255f, 255 / 255f);
            var ps = _bigFlower[1].GetComponent<ParticleSystem>();
            ParticleSystem.ColorOverLifetimeModule _colt = ps.colorOverLifetime;
            _colt.color = _a;

            Color _b = new Color(255 / 255f, 79 / 255f, 112 / 255f, 255 / 255f);
            _bigFlower[2].startColor = _b;

            Color _c = new Color(255 / 255f, 99 / 255f, 135 / 255f, 255 / 255f);
            _bigFlower[3].startColor = _c;

            Color _d = new Color(255 / 255f, 99 / 255f, 206 / 255f, 197 / 255f);
            _bigFlower[4].startColor = _d;

            Color _e = new Color(255 / 255f, 64 / 255f, 139 / 255f, 128 / 255f);
            _bigFlower[5].startColor = _e;

            Color _f = new Color(255 / 255f, 64 / 255f, 139 / 255f, 96 / 255f);
            _bigFlower[6].startColor = _f;

            Color _g = new Color(255 / 255f, 82 / 255f, 165 / 255f, 255 / 255f);
            _bigFlower[7].startColor = _g;

            Color _h = new Color(255 / 255f, 124 / 255f, 175 / 255f, 255 / 255f);
            _bigFlower[8].startColor = _h;

            Color _i = new Color(255 / 255f, 58 / 255f, 127 / 255f, 255 / 255f);
            _bigFlower[9].startColor = _i;

            Color _j = new Color(255 / 255f, 82 / 255f, 193 / 255f, 255 / 255f);

            var ps2 = _bigFlower[10].GetComponent<ParticleSystem>();
            var col = ps2.colorOverLifetime;
            col.color = new ParticleSystem.MinMaxGradient(_j, Color.white);

            Color _k = new Color(255 / 255f, 73 / 255f, 175 / 255f, 128 / 255f);
            _bigFlower[11].startColor = _k;


        }

        public void OneDollarShow()
        {
        }

        public void FiveDollarShow()
        {
            SetDefoultColors();
            _showDonationFlower.SetActive(true);
        }

        public void TenDollarShow()
        {
            CreateRandomFlower();
            _trail_L.SetActive(true);
            _trail_R.SetActive(true);
            _showDonationFlower.SetActive(true);
        }


        public void StartShow()
    {
            int _sum = _donationManager.DonationAmount;
            SoundyManager.Play(_soundData);
            _showDonationCamera.SetActive(true);
            switch(_sum)
            {
                case 1:
                    OneDollarShow();
                    break;
                case 5:
                    FiveDollarShow();
                    break;
                case 10:
                    TenDollarShow();
                    break;
            }
    }

    public void StopShow()
    {
            _showDonationCamera.SetActive(false);
            _trail_L.SetActive(false);
            _trail_R.SetActive(false);
            _showDonationFlower.SetActive(false);
        }
}
}
