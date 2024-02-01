import { useEffect, useState } from "react";
import reactLogo from "./assets/react.svg";
import viteLogo from "/vite.svg";
import "./App.css";
import Header from "./components/Header";
import ListGroup from "./components/ListGroup";

function App() {
  const items = ["New York", "Kyiv", "Paris", "London", "Tokyo"];

  const handleSelectCategoryItem = (item: string, index: number) => {
    var categoryData = serviceResponseCategories[index];
    var subCategories = categoryData.subCategories;
    if (subCategories != null) {
      SetListCategories(subCategories);
      SetSubcategoryEnabled(true);
      SetCurrentCategory(categoryData.name);
    }
  };

  const [wasLoaded, SetWasLoaded] = useState(false);
  const [subcategoryEnabled, SetSubcategoryEnabled] = useState(false);
  const [serviceResponseCategories, SetServiceResponseCategories] = useState(
    [] as ProductCategory[]
  );
  const [listCategories, SetListCategories] = useState([] as string[]);
  const [currentCategory, SetCurrentCategory] = useState("");

  const apiUrl = "http://localhost:5247/";

  useEffect(() => {
    const fetchData = async () => {
      const result = await fetch(apiUrl + "productcategories");
      result.json().then((jsonData: ProductCategory[]) => {
        console.log(jsonData);
        SetServiceResponseCategories(jsonData);
        SetListCategories(jsonData.map((x) => x.name));
        SetWasLoaded(true);
      });
    };
    fetchData();
  }, []);

  return (
    <>
      {wasLoaded ? (
        <>
          <Header></Header>
          {!subcategoryEnabled ? (
            <ListGroup
              heading="Categories"
              items={listCategories}
              onSelectItem={handleSelectCategoryItem}
            ></ListGroup>
          ) : (
            <ListGroup
              heading={currentCategory}
              items={listCategories}
              onSelectItem={handleSelectCategoryItem}
            ></ListGroup>
          )}
        </>
      ) : (
        <h1>Data not loaded yet</h1>
      )}
    </>
  );
}

export default App;
