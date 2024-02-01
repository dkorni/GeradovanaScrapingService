import { Fragment, MouseEvent, useState } from "react";

interface ListGroupProps {
  items: string[];
  heading: string;
  onSelectItem: (item: string, index: number) => void;
}

function ListGroup({ items, heading, onSelectItem }: ListGroupProps) {
  const [selectedIndex, setSelectedIndex] = useState(-1);

  const handleClick = (item: string, index: number) => {
    setSelectedIndex(index);
    onSelectItem(item, index);
  };

  return (
    <Fragment>
      <h4>{heading}</h4>
      <ul className="list-group">
        {items.map((item, index) => (
          <li
            className={
              selectedIndex === index
                ? "list-group-item active"
                : "list-group-item"
            }
            onClick={() => handleClick(item, index)}
          >
            {item}
          </li>
        ))}
      </ul>
    </Fragment>
  );
}

export default ListGroup;
