using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace FancyScrollView
{
    public class ListenerEvent : MonoBehaviour
    {
        public delegate void TriggerDelegate(GameObject go, BaseEventData data);

        TriggerDelegate triggerEvent;

        protected bool EventEnable()
        {
            if (!isActiveAndEnabled)
                return false;

            return true;
        }

        public void Add(TriggerDelegate d)
        {
            triggerEvent += d;
        }

        public void Remove(TriggerDelegate d)
        {
            triggerEvent -= d;
        }

        public void Clear()
        {
            triggerEvent = null;
        }

        public void Invoke(GameObject go, BaseEventData data)
        {
            if (triggerEvent != null)
            {
                triggerEvent(go, data);
            }
        }
    }

    public class IPointerEnterEvent : ListenerEvent, IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (EventEnable())
                this.Invoke(gameObject, eventData);
        }
    }

    public class IPointerEixtEvent : ListenerEvent, IPointerExitHandler
    {
        public void OnPointerExit(PointerEventData eventData)
        {
            if (EventEnable())
                this.Invoke(gameObject, eventData);
        }
    }

    public class IScrollEvent : ListenerEvent, IScrollHandler
    {
        public void OnScroll(PointerEventData eventData)
        {
            if (EventEnable())
                this.Invoke(gameObject, eventData);
        }
    }

    public class IDropEvent : ListenerEvent, IDropHandler
    {

        public void OnDrop(PointerEventData eventData)
        {
            if (EventEnable())
                this.Invoke(gameObject, eventData);
        }
    }

    public class IEndDragEvent : ListenerEvent, IEndDragHandler
    {
        public void OnEndDrag(PointerEventData eventData)
        {
            if (EventEnable())
                this.Invoke(gameObject, eventData);
        }
    }

    public class IDragEvent : ListenerEvent, IDragHandler
    {
        public void OnDrag(PointerEventData eventData)
        {
            if (EventEnable())
                this.Invoke(gameObject, eventData);
        }
    }

    public class IBeginDragtEvent : ListenerEvent, IBeginDragHandler
    {

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (EventEnable())
                this.Invoke(gameObject, eventData);
        }
    }

    public class IPointerClicEvent : ListenerEvent, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            if (EventEnable())
                this.Invoke(gameObject, eventData);
        }
    }

    public class IPointerUpEvent : ListenerEvent, IPointerUpHandler
    {

        public void OnPointerUp(PointerEventData eventData)
        {
            if (EventEnable())
                this.Invoke(gameObject, eventData);
        }
    }

    public class IPointerDownEvent : ListenerEvent, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            if (EventEnable())
                this.Invoke(gameObject, eventData);
        }
    }

    public class IUpdateSelectedEvent : ListenerEvent, IUpdateSelectedHandler
    {
        public void OnUpdateSelected(BaseEventData eventData)
        {
            if (EventEnable())
                this.Invoke(gameObject, eventData);
        }
    }

    public class ISubmitEvent : ListenerEvent, ISubmitHandler
    {
        public void OnSubmit(BaseEventData eventData)
        {
            if (EventEnable())
                this.Invoke(gameObject, eventData);
        }
    }

    public class IMoveEvent : ListenerEvent, IMoveHandler
    {
        public void OnMove(AxisEventData eventData)
        {
            if (EventEnable())
                this.Invoke(gameObject, eventData);
        }
    }

    public class IDeselectEvent : ListenerEvent, IDeselectHandler
    {
        public void OnDeselect(BaseEventData eventData)
        {
            if (EventEnable())
                this.Invoke(gameObject, eventData);
        }
    }

    public class ISelectEvent : ListenerEvent, ISelectHandler
    {
        public void OnSelect(BaseEventData eventData)
        {
            if (EventEnable())
                this.Invoke(gameObject, eventData);
        }

    }

    public class ICancelEvent : ListenerEvent, ICancelHandler
    {

        public void OnCancel(BaseEventData eventData)
        {
            if (EventEnable())
                this.Invoke(gameObject, eventData);
        }

    }

    public class UILisenter : MonoBehaviour
    {
        private ListenerEvent _onPointerEnter;
        public ListenerEvent onPointerEnter
        {
            get
            {
                if (_onPointerEnter == null)
                    _onPointerEnter = this.gameObject.AddComponent<IPointerEnterEvent>();
                return _onPointerEnter;
            }
        }

        private ListenerEvent _onPointerExit;
        public ListenerEvent onPointerExit
        {
            get
            {
                if (_onPointerExit == null)
                    _onPointerExit = this.gameObject.AddComponent<IPointerEixtEvent>();
                return _onPointerExit;
            }
        }

        private ListenerEvent _onDrag;
        public ListenerEvent onDrag
        {
            get
            {
                if (_onDrag == null)
                    _onDrag = this.gameObject.AddComponent<IDragEvent>();
                return _onDrag;
            }
        }

        private ListenerEvent _onDrop;
        public ListenerEvent onDrop
        {
            get
            {
                if (_onDrop == null)
                    _onDrop = this.gameObject.AddComponent<IDropEvent>();
                return _onDrop;
            }
        }

        private ListenerEvent _onPointerDown;
        public ListenerEvent onPointerDown
        {
            get
            {
                if (_onPointerDown == null)
                    _onPointerDown = this.gameObject.AddComponent<IPointerDownEvent>();
                return _onPointerDown;
            }
        }

        private ListenerEvent _onPointerUp;
        public ListenerEvent onPointerUp
        {
            get
            {
                if (_onPointerUp == null)
                    _onPointerUp = this.gameObject.AddComponent<IPointerUpEvent>();
                return _onPointerUp;
            }
        }

        private ListenerEvent _onPointerClick;
        public ListenerEvent onPointerClick
        {
            get
            {
                if (_onPointerClick == null)
                    _onPointerClick = this.gameObject.AddComponent<IPointerClicEvent>();
                return _onPointerClick;
            }
        }

        private ListenerEvent _onSelect;
        public ListenerEvent onSelect
        {
            get
            {
                if (_onSelect == null)
                    _onSelect = this.gameObject.AddComponent<ISelectEvent>();
                return _onSelect;
            }
        }

        private ListenerEvent _onDeselect;
        public ListenerEvent onDeselect
        {
            get
            {
                if (_onDeselect == null)
                    _onDeselect = this.gameObject.AddComponent<IDeselectEvent>();
                return _onDeselect;
            }
        }

        private ListenerEvent _onScroll;
        public ListenerEvent onScroll
        {
            get
            {
                if (_onScroll == null)
                    _onScroll = this.gameObject.AddComponent<IScrollEvent>();
                return _onScroll;
            }
        }

        private ListenerEvent _onMove;
        public ListenerEvent onMove
        {
            get
            {
                if (_onMove == null)
                    _onMove = this.gameObject.AddComponent<IMoveEvent>();
                return _onMove;
            }
        }

        private ListenerEvent _onUpdateSelected;
        public ListenerEvent onUpdateSelected
        {
            get
            {
                if (_onUpdateSelected == null)
                    _onUpdateSelected = this.gameObject.AddComponent<IUpdateSelectedEvent>();
                return _onUpdateSelected;
            }
        }

        private ListenerEvent _onBeginDrag;
        public ListenerEvent onBeginDrag
        {
            get
            {
                if (_onBeginDrag == null)
                    _onBeginDrag = this.gameObject.AddComponent<IBeginDragtEvent>();
                return _onBeginDrag;
            }
        }

        private ListenerEvent _onEndDrag;
        public ListenerEvent onEndDrag
        {
            get
            {
                if (_onEndDrag == null)
                    _onEndDrag = this.gameObject.AddComponent<IEndDragEvent>();
                return _onEndDrag;
            }
        }

        private ListenerEvent _onSubmit;
        public ListenerEvent onSubmit
        {
            get
            {
                if (_onSubmit == null)
                    _onSubmit = this.gameObject.AddComponent<ISubmitEvent>();
                return _onSubmit;
            }
        }

        private ListenerEvent _onCancel;
        public ListenerEvent onCancel
        {
            get
            {
                if (_onCancel == null)
                    _onCancel = this.gameObject.AddComponent<ICancelEvent>();
                return _onCancel;
            }
        }
    }
}



