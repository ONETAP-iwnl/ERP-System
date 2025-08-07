'use client'

import { useRouter } from 'next/navigation';
import { useEffect, useState } from 'react';
import styles from "@/styles/recipts-page/hero/hero.module.css"
import { getUnitById, updateUnit, deleteUnit, archiveUnit } from '@/services/api/unitClient';

interface Unit {
  id: number;
  name: string;
  isActive: boolean;
}

interface HeroProps {
  id: string;
}

export default function Hero({ id }: HeroProps) {
  const router = useRouter();
  const [unit, setUnit] = useState<Unit | null>(null);
  const [inputName, setInputName] = useState('');

    useEffect(() => {
    const unitId = parseInt(id);
    console.log("Получен ID:", unitId);

    if (isNaN(unitId)) {
      console.error("Некорректный ID:", id);
      return;
    }

    async function fetchUnit() {
      try {
        const data = await getUnitById(unitId);
        console.log("Получена единица:", data);
        setUnit(data);
        setInputName(data.name);
      } catch (error) {
        console.error("Ошибка при получении единицы измерения", error);
      }
    }

    fetchUnit();
  }, [id]);


  const handleSave = async () => {
    if (!unit) return;
    try {
      await updateUnit(unit.id, inputName, true);
      router.push('/unit-page');
    } catch (error) {
      console.error('Ошибка при сохранении:', error);
    }
  };

  const handleDelete = async () => {
    if (!unit) return;
    try {
      await deleteUnit(unit.id);
      router.push('/unit-page');
    } catch (error) {
      console.error('Ошибка при удалении:', error);
    }
  };

  const handleArchive = async () => {
    if (!unit) return;
    try {
      await archiveUnit(unit.id);
      router.push('/unit-page');
    } catch (error) {
      console.error('Ошибка при архивировании:', error);
    }
  };

  if (!unit) return <div>Загрузка...</div>;
        return (
        <main className={styles.mainContainer}>
            <h1 className={styles.headerText}>Единица измерения</h1>
            <div className={styles.buttonGroup}>
                <button className={styles.addButton} onClick={handleSave}>Сохранить</button>
                <button className={styles.deleteButton} onClick={handleDelete}>Удалить</button>
                <button className={styles.archiveButton} onClick={handleArchive}>В архив</button>
            </div>
            <div className={styles.inputGroup}>
                <h4 className={styles.filterText}>Наименование</h4>
                <input
                    type='text'
                    className={styles.inputNumber}
                    value={inputName}
                    onChange={(e) => setInputName(e.target.value)}
                />
            </div>
        </main>
  );
}